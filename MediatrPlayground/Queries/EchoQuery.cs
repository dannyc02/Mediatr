using MediatR;

namespace MediatrPlayground.Queries
{
    public class EchoQuery : CacheableQuery, IRequest<string>
    {
        public override string VaryByQueryParameters => TextToEcho;
        public string TextToEcho { get; set; }
    }

    public abstract class CacheableQuery : ICacheableRequest
    {
        public virtual string VaryByQueryParameters { get; }
        public virtual bool VaryBySomething { get; }
        public virtual bool VaryBySomethingElse { get; }
    }

    public class EchoQueryHandler : IRequestHandler<EchoQuery, string>
    {
        public string Handle(EchoQuery request)
        {
            return request.TextToEcho;
        }
    }

    public interface ICacheableRequest
    {
    }
}