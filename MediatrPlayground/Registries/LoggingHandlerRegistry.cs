using MediatR;
using NLog;
using StructureMap;
using System;

namespace MediatrPlayground.Registries
{
    public class LoggingHandlerRegistry : Registry
    {
        public LoggingHandlerRegistry()
        {
            var handlerType = For(typeof(IRequestHandler<,>));
            handlerType.DecorateAllWith(typeof(CachingHandler<,>));
        }
    }

    public class LoggingHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _inner;
        private readonly ILogger _logger;

        public LoggingHandler(IRequestHandler<TRequest, TResponse> inner, ILogger logger)
        {
            _inner = inner;
            _logger = logger;
        }

        public TResponse Handle(TRequest message)
        {
            Console.WriteLine("Before");
            var before = $"Starting request: {message.GetType().Name}.";
            _logger.Info("");
            var result = _inner.Handle(message);
            Console.WriteLine("After");
            return result;
        }
    }
}