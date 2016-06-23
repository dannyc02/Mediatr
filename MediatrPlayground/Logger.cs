using MediatR;
using System;

namespace MediatrPlayground
{
    public class Logger<TRequest, TResponse> : ILogger<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public void LogInfo(TRequest request, TResponse response)
        {
            throw new NotImplementedException();
        }

        public void LogError(TRequest request, TResponse response, Exception ex)
        {
            throw new NotImplementedException();
        }

        public void LogRequestInfo<TRequest1, TResponse1>(TRequest1 request) where TRequest1 : IRequest<TResponse1>
        {
            throw new NotImplementedException();
        }

        public void LogResponseInfo<TResponse1>(TResponse1 response)
        {
            throw new NotImplementedException();
        }

        public void LogSql(string sql)
        {
            throw new NotImplementedException();
        }
    }

    public interface ILogger<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        void LogInfo(TRequest request, TResponse response);

        void LogError(TRequest request, TResponse response, Exception ex);

        void LogRequestInfo<TRequest, TResponse>(TRequest request) where TRequest : IRequest<TResponse>;

        void LogResponseInfo<TResponse>(TResponse response);

        void LogSql(string sql);
    }
}