package com.zynga.core.platform;

public class DeviceInfoResult {
    public String stringResult;
    public boolean boolResult;
    public double doubleResult;
    public String exceptionString;
    
    private void GenerateExceptionString(Throwable exception)
    {
        if (exception != null)
        {
            String exceptionMessage = exception.getMessage();
            StringBuilder message = new StringBuilder(exceptionMessage != null ? exceptionMessage : "");
            message.append("\nStack trace:\n");
            StackTraceElement[] stackTraceElements = exception.getStackTrace();
            for (StackTraceElement stackTraceElement : stackTraceElements)
            {
                message.append(stackTraceElement.getMethodName());
                message.append(" in class ");
                message.append(stackTraceElement.getClassName());
                message.append(" [on line number ");
                message.append(stackTraceElement.getLineNumber());
                message.append(" of file ");
                message.append(stackTraceElement.getFileName());
                message.append("]\n");
            }
            exceptionString = message.toString();
        }
    }

    public DeviceInfoResult(String result, Throwable exception)
    {
        stringResult = result;
        GenerateExceptionString(exception);
    }
    public DeviceInfoResult(boolean result, Throwable exception)
    {
        boolResult = result;
        GenerateExceptionString(exception);
    }
    public DeviceInfoResult(double result, Throwable exception)
    {
        doubleResult = result;
        GenerateExceptionString(exception);
    }
}
    
