using MediatR;
using MediatrPlayground.Queries;
using ServiceStack.Caching;
using StructureMap;
using System.Text;

namespace MediatrPlayground.Registries
{
    public class CacheHandlerRegistry : Registry
    {
        public CacheHandlerRegistry()
        {
            var handlerType = For(typeof(IRequestHandler<,>));
            handlerType.DecorateAllWith(typeof(CachingHandler<,>));
        }
    }

    public class CachingHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _inner;
        private readonly ICacheClient _cacheClient;

        public CachingHandler(IRequestHandler<TRequest, TResponse> inner, ICacheClient cacheClient)
        {
            _inner = inner;
            _cacheClient = cacheClient;
        }

        public TResponse Handle(TRequest request)
        {
            if (request is ICacheableRequest)
            {
                var cacheKey = GenerateCacheKey(request);
                var cachedResult = _cacheClient.Get<TResponse>(cacheKey);
                if (cachedResult == null)
                {
                    var result = _inner.Handle(request);
                    _cacheClient.Set(cacheKey, result);
                    return result;
                }
                return cachedResult;
            }

            return _inner.Handle(request);
        }

        private string GenerateCacheKey(TRequest request)
        {
            var builder = new StringBuilder(request.GetType().Name);

            return builder.ToString();
        }
    }
}