using System;
using System.Text;
using System.Text.RegularExpressions;

#if WINDOWS_UWP
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
#else
using System.Security.Cryptography;
#endif

namespace Game.Utility
{

	public enum UnicodeSpace
	{

		// Listed in roughly descending order by width as observed in Helvetica on iOS.
		// Derived from: http://en.wikipedia.org/wiki/Space_%28punctuation%29

		// Wider than a normal space
		Ideographic = 0x3000,
		Em = 0x2003,
		Figure = 0x2007,
		ThreePerEm = 0x2004,

		// As wide as a normal space
		Normal = 0x0020,
		En = 0x2002,
		FourPerEm = 0x2005,
		MediumMathematical = 0x205F,

		// Narrower than a normal space
		SixPerEm = 0x2006,
		NarrowNoBreak = 0x202F,
		Punctuation = 0x2008,
		Thin = 0x2009,
		Hair = 0x200A
	}

	public static class StringUtils
	{

		// Splits a string into two lines around the space character closest to the center.
		public static bool Bisect(string str, out string line1, out string line2)
		{

			line1 = null;
			line2 = null;

			int splitIndex;
			bool canSplit = Bisect(str, out splitIndex);

			if (canSplit)
			{
				line1 = str.Substring(0, splitIndex);
				line2 = str.Substring(splitIndex + 1, str.Length - splitIndex - 1);
			}

			return canSplit;
		}

		// If successful, "splitIndex" is the position of the space character closest to the middle of the string.
		public static bool Bisect(string str, out int splitIndex)
		{
			return Split(str, 0.5f, out splitIndex);
		}

		public static bool Trisect(string str, out string line1, out string line2, out string line3)
		{

			line1 = null;
			line2 = null;
			line3 = null;

			// Try splitting the head third off first before bisecting the tail two-thirds
			int splitIndex;
			bool canSplit = Split(str, 1f / 3f, out splitIndex);

			if (canSplit)
			{

				line1 = str.Substring(0, splitIndex);
				string remainder = str.Substring(splitIndex + 1, str.Length - splitIndex - 1);
				canSplit = Bisect(remainder, out line2, out line3);

				// Try splitting the tail third off first before bisecting the head two-thirds
				if (!canSplit)
				{

					canSplit = Split(str, 2f / 3f, out splitIndex);

					if (canSplit)
					{
						line3 = str.Substring(splitIndex + 1, str.Length - splitIndex - 1);
						remainder = str.Substring(0, splitIndex);
						canSplit = Bisect(remainder, out line1, out line2);
					}
				}
			}

			return canSplit;
		}

		// If successful, "splitIndex" is the position of the space character closest to the specified fractional length.
		public static bool Split(string str, float fractionalLength, out int splitIndex)
		{

			if ((fractionalLength < 0) || (fractionalLength > 1)) throw new ArgumentException("Fractional length must be [0..1]");

			splitIndex = -1;

			if (!str.Contains(" ")) return false;

			// Determine the closest point on which to break into two lines
			int idealSplitIndex = (int)(str.Length * fractionalLength);
			int offset = 0;

			while (true)
			{
				if ((idealSplitIndex - offset >= 0) && (str[idealSplitIndex - offset] == ' '))
				{
					splitIndex = idealSplitIndex - offset;
					break;
				}
				else if ((idealSplitIndex + offset < str.Length) && (str[idealSplitIndex + offset] == ' '))
				{
					splitIndex = idealSplitIndex + offset;
					break;
				}
				offset++;
			}

			return true;
		}

		public static string GetSpace(UnicodeSpace space)
		{
			return Char.ConvertFromUtf32((int)space);
		}

#if WINDOWS_UWP
		public static byte[] Digest(string message) {

			HashAlgorithmProvider sha = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);
			IBuffer hashBuffer = sha.HashData(CryptographicBuffer.ConvertStringToBinary(message, BinaryStringEncoding.Utf8));
			byte[] result = new byte[hashBuffer.Length];
			CryptographicBuffer.CopyToByteArray(hashBuffer, out result);

			return result;
		}
#else
		private static SHA1CryptoServiceProvider sha;

		public static byte[] Digest(string message)
		{

			if (sha == null)
			{
				sha = new SHA1CryptoServiceProvider();
			}

			byte[] data = Encoding.UTF8.GetBytes(message);

			return sha.ComputeHash(data);
		}
#endif

		public static string DigestToHex(string message)
		{
			return StringUtils.BytesToHex(Digest(message));
		}

		public static string DigestToBase64(string message)
		{
			return Convert.ToBase64String(Digest(message));
		}

		public static string BytesToHex(byte[] bytes)
		{

			// Fast:
			// http://stackoverflow.com/questions/311165/how-do-you-convert-byte-array-to-hexadecimal-string-and-vice-versa-in-c

			char[] c = new char[bytes.Length * 2];

			byte b;
			for (int i = 0; i < bytes.Length; i++)
			{
				b = ((byte)(bytes[i] >> 4));
				c[i * 2] = (char)(b > 9 ? b + 0x37 : b + 0x30);
				b = ((byte)(bytes[i] & 0xF));
				c[i * 2 + 1] = (char)(b > 9 ? b + 0x37 : b + 0x30);
			}

			return new string(c);
		}

		/*
		public static void MeasureSpaces(UIFont font, UIView control, TextWriter output) {
			
			string[] names = Enum.GetNames(typeof(UnicodeSpace));
			
			int i=0;
			foreach (int codepoint in Enum.GetValues(typeof(UnicodeSpace))) {
				string val = Char.ConvertFromUtf32(codepoint);
				SizeF size = control.StringSize(val, font);
				output.WriteLine(names[i] + ": [" + val + "]" + size.Width);
				i++;
			}
		}
		*/

		public static string OrdinalSuffix(this int value)
		{

			string extension = "th";

			int last_digits = value % 100;

			if (last_digits < 11 || last_digits > 13)
			{
				switch (last_digits % 10)
				{
					case 1:
						extension = "st";
						break;
					case 2:
						extension = "nd";
						break;
					case 3:
						extension = "rd";
						break;
				}
			}

			return extension;
		}
	}

	public class HTMLTagStripper
	{

		private MatchEvaluator tagFilter;

		public HTMLTagStripper() : this(null) { }

		public HTMLTagStripper(string[] allowedTags)
		{
			this.AllowedTags = allowedTags;
			this.tagFilter = new MatchEvaluator(FilterHTMLTags);
		}

		// Specify a whitelist of allowed tags, lowercase, without the angle brackets: e.g. {"b", "i"}
		public string[] AllowedTags { get; set; }

		public string StripTags(string str)
		{
			return HTMLTagRegex.Replace(str, this.tagFilter);
		}

		private string FilterHTMLTags(Match m)
		{

			if ((this.AllowedTags != null) && (this.AllowedTags.Length > 0))
			{
				string matchedTag = m.Groups[1].Value.ToLower();
				foreach (string allowedTag in this.AllowedTags)
				{
					if (matchedTag == allowedTag) return m.Value;
				}
			}

			return String.Empty;
		}

		private static Regex htmlTagRegex;

		private static Regex HTMLTagRegex
		{
			get
			{
				if (htmlTagRegex == null)
				{
					htmlTagRegex = new Regex("</?(.+?) */?>");
				}
				return htmlTagRegex;
			}
		}
	}
}
