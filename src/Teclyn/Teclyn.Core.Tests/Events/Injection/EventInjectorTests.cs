//using System;
//using Teclyn.Core.Events;
//using Teclyn.Core.Events.Injection;
//using Xunit;

//namespace Teclyn.Core.Tests.Events.Injection
//{
//    public class EventInjectorTests
//    {
//        private readonly EventInjector eventInjector = new EventInjector();

//        [AttributeUsage(AttributeTargets.Property)]
//        private class EventInjectorTestAttribute : Attribute { }

//        private class TestEvent : ITeclynEvent
//        {
//            public string AggregateId { get; set; }

//            [EventInjectorTest]
//            public string InjectedProperty { get; set; }
//            public string NotInjectedProperty { get; set; }
//            [EventInjectorTest]
//            private string PrivateInjectedProperty { get; set; }

//            public string GetPrivateInjectedProperty()
//            {
//                return this.PrivateInjectedProperty;
//            }
//        }

//        public EventInjectorTests()
//        {
//            var teclyn = TeclynApi.Initialize(new TeclynTestConfiguration(), false);
//        }

//        [Fact]
//        public void RegisteredAttributeIsInjected()
//        {
//            var value = "injected";
//            eventInjector.RegisterAttribute<EventInjectorTestAttribute, string>(() => value);
//            var injectedObject = new TestEvent();

//            eventInjector.Inject(injectedObject);

//            Assert.Equal(value, injectedObject.InjectedProperty);
//        }

//        [Fact]
//        public void PrivateRegisteredAttributeIsInjected()
//        {
//            var value = "injected";
//            eventInjector.RegisterAttribute<EventInjectorTestAttribute, string>(() => value);
//            var injectedObject = new TestEvent();

//            eventInjector.Inject(injectedObject);

//            Assert.Equal(value, injectedObject.GetPrivateInjectedProperty());
//        }

//        [Fact]
//        public void NotRegisteredAttributeIsNotInjected()
//        {
//            var value = "injected";
//            eventInjector.RegisterAttribute<EventInjectorTestAttribute, string>(() => value);
//            var injectedObject = new TestEvent();

//            eventInjector.Inject(injectedObject);

//            Assert.Null(injectedObject.NotInjectedProperty);
//        }
//    }
//}