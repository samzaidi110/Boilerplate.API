using System;

namespace Boilerplate.Application.Common;
public class BusinessRuleException : Exception
{
    public int ResponseCode { get; }
    public string ResponseMessage { get; }

    public BusinessRuleException(int responseCode, string responseMessage)
        : base(responseMessage)
    {
        ResponseCode = responseCode;
        ResponseMessage = responseMessage;
    }
}