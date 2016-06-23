using FluentAssertions;
using MediatR;
using MediatrPlayground.Queries;
using MediatrPlayground.Registries;
using NUnit.Framework;
using StructureMap;
using System.Collections.Generic;
using TestStack.BDDfy;

namespace MediatrPlayground
{
    [TestFixture]
    public class RequestTests
    {
        private Container _container;
        private IList<string> _results;

        [SetUp]
        public void SetUp()
        {
            _container = new Container();
            _results = new List<string>();
        }

        [Test]
        public void AssertConfiguration()
        {
            _container.AssertConfigurationIsValid();
        }

        [Test]
        public void Query_WithBasicImplementation_CallsHandler()
        {
            this.Given(x => TheContainerHasRegistries(new DefaultRegistry()))
                .When(x => TheQueryIsRun(new PingQuery()))
                .Then(x => TheResultsShouldBe("Pong"))
                .BDDfy();
        }

        [Test]
        public void QueryTwice_WithNoCaching_ReturnsDifferentResults()
        {

            this.Given(x => TheContainerHasRegistries(new DefaultRegistry()))
                .When(x => TheQueryIsRun(new EchoQuery() { TextToEcho = "Call#1" }))
                    .And(x => TheQueryIsRun(new EchoQuery() { TextToEcho = "Call#2" }))
                .Then(x => x.TheResultsShouldBe("Call#1", "Call#2"))
                .BDDfy();
        }

        private void ACachableQueryShouldHaveTheCachingHandlerDecorator()
        {
            var handler = _container.GetInstance<IRequestHandler<EchoQuery, string>>();
            handler.Should().BeOfType<CachingHandler<EchoQuery, string>>();
        }

        [Test]
        public void QueryTwice_WithCaching_ReturnsCachedResult()
        {
            this.Given(x => TheContainerHasRegistries(new DefaultRegistry(), new CacheHandlerRegistry()))
                .When(x => TheQueryIsRun(new EchoQuery() { TextToEcho = "Call#1" }))
                    .And(x => TheQueryIsRun(new EchoQuery() { TextToEcho = "Call#2" }))
                .Then(x => x.TheResultsShouldBe("Call#1", "Call#1"))
                .BDDfy();
        }

        [Test]
        public void AddingCacheDecorator_EnsureHandlersAddedToContainer()
        {
            this.Given(x => TheContainerHasRegistries(new DefaultRegistry(), new CacheHandlerRegistry()))
                .Then(x => x.ACachableQueryShouldHaveTheCachingHandlerDecorator())
                .BDDfy();
        }

        private void TheResultsShouldBe(params string[] expectedResult)
        {
            _results.ShouldAllBeEquivalentTo(expectedResult);
        }

        private void TheQueryIsRun(IRequest<string> request)
        {
            var mediator = _container.GetInstance<IMediator>();
            var result = mediator.Send(request);
            _results.Add(result);
        }

        private void TheContainerHasRegistries(params Registry[] registries)
        {
            var mainRegistry = new Registry();
            foreach (var registry in registries)
            {
                mainRegistry.IncludeRegistry(registry);
            }

            _container.Configure(cfg => cfg.AddRegistry(mainRegistry));
        }
    }
}
