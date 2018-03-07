using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teclyn.Core.Api
{
    public class AggregateInfo
    {
        public string Id { get; }
        public string Name { get; }
        public Type AggregateType { get; }
        public Type ImplementationType { get; }
        public Type RepositoryType { get; }
        public Type RepositoryImplementationType { get; }
        public Type RepositoryProviderType { get; }
        public Type RepositoryProviderImplementationType { get; }
        public string CollectionName { get; }
        public Type DefaultFilterType { get; }
        public Type AccessControllerType { get; }

        public AggregateInfo(
            string id, 
            string name, 
            Type aggregateType, 
            Type implementationType, 
            Type repositoryType, 
            Type repositoryImplementationType,
            Type repositoryProviderType,
            Type repositoryProviderImplementationType,
            string collectionName,
            Type accessControllerType,
            Type defaultFilterType)
        {
            this.Id = id;
            this.Name = name;
            this.AggregateType = aggregateType;
            this.ImplementationType = implementationType;
            this.RepositoryType = repositoryType;
            this.RepositoryImplementationType = repositoryImplementationType;
            this.RepositoryProviderType = repositoryProviderType;
            this.RepositoryProviderImplementationType = repositoryProviderImplementationType;
            this.CollectionName = collectionName;
            this.AccessControllerType = accessControllerType;
            this.DefaultFilterType = defaultFilterType;
        }
    }
}