using MediatR;

namespace MediatrPlayground.Queries
{
    public class PingQuery : IRequest<string>
    {
    }

    public class PingQueryHandler : IRequestHandler<PingQuery, string>
    {
        public string Handle(PingQuery request)
        {
            return "Pong";
        }
    }
}