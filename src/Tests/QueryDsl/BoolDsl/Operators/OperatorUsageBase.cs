using System;
using FluentAssertions;
using Nest;
using Tests.Framework.MockData;

namespace Tests.QueryDsl.BoolDsl.Operators
{
	public abstract class OperatorUsageBase
	{
		protected void ReturnsNull(QueryContainer combined, Func<QueryContainerDescriptor<Project>, QueryContainer> selector)
		{
			combined.Should().BeNull(); selector.InvokeQuery(new QueryContainerDescriptor<Project>()).Should().BeNull();
		}

		protected void ReturnsBool(QueryContainer combined, Func<QueryContainerDescriptor<Project>, QueryContainer> selector, Action<IBoolQuery> boolQueryAssert)
		{
			ReturnsBool(combined, boolQueryAssert);
			ReturnsBool(selector.InvokeQuery(new QueryContainerDescriptor<Project>()), boolQueryAssert);
		}

		private void ReturnsBool(QueryContainer combined, Action<IBoolQuery> boolQueryAssert)
		{
			combined.Should().NotBeNull();
			IQueryContainer c = combined;
			c.Bool.Should().NotBeNull();
			boolQueryAssert(c.Bool);
		}

		protected void ReturnsSingleQuery(QueryContainer combined, Func<QueryContainerDescriptor<Project>, QueryContainer> selector, Action<IQueryContainer> containerAssert)
		{
			ReturnsSingleQuery(combined, containerAssert);
			ReturnsSingleQuery(selector.InvokeQuery(new QueryContainerDescriptor<Project>()), containerAssert);
		}

		private void ReturnsSingleQuery(QueryContainer combined, Action<IQueryContainer> containerAssert)
		{
			combined.Should().NotBeNull();
			IQueryContainer c = combined;
			containerAssert(c);
		}
	}
}