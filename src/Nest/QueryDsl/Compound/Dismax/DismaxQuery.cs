﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Nest
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	//[JsonConverter(typeof(DismaxQueryJsonConverter))]
	[JsonConverter(typeof(ReadAsTypeJsonConverter<DismaxQuery>))]
	public interface IDisMaxQuery : IQuery
	{
		[JsonProperty(PropertyName = "tie_breaker")]
		double? TieBreaker { get; set; }

		[JsonProperty(PropertyName = "queries")]
		IEnumerable<QueryContainer> Queries { get; set; }
	}

	public class DismaxQuery : QueryBase, IDisMaxQuery
	{
		bool IQuery.Conditionless => IsConditionless(this);
		public double? TieBreaker { get; set; }
		public IEnumerable<QueryContainer> Queries { get; set; }

		protected override void WrapInContainer(IQueryContainer c) => c.DisMax = this;
		internal static bool IsConditionless(IDisMaxQuery q) => !q.Queries.HasAny() || q.Queries.All(qq => qq.IsConditionless);
	}

	public class DisMaxQueryDescriptor<T> 
		: QueryDescriptorBase<DisMaxQueryDescriptor<T>, IDisMaxQuery>
		, IDisMaxQuery where T : class
	{
		bool IQuery.Conditionless => DismaxQuery.IsConditionless(this);
		double? IDisMaxQuery.TieBreaker { get; set; }
		IEnumerable<QueryContainer> IDisMaxQuery.Queries { get; set; }

		public DisMaxQueryDescriptor<T> Queries(params Func<QueryContainerDescriptor<T>, QueryContainer>[] querySelectors) => 
			Assign(a => a.Queries = querySelectors.Select(q=>q(new QueryContainerDescriptor<T>())).Where(q => !q.IsConditionless).ToListOrNullIfEmpty());

		public DisMaxQueryDescriptor<T> Queries(IEnumerable<Func<QueryContainerDescriptor<T>, QueryContainer>> querySelectors) => 
			Assign(a => a.Queries = querySelectors.Select(q=>q(new QueryContainerDescriptor<T>())).Where(q => !q.IsConditionless).ToListOrNullIfEmpty());

		public DisMaxQueryDescriptor<T> Queries(params QueryContainer[] queries) => Assign(a => a.Queries = queries.Where(q => !q.IsConditionless).ToListOrNullIfEmpty());

		public DisMaxQueryDescriptor<T> TieBreaker(double? tieBreaker) => Assign(a => a.TieBreaker = tieBreaker);
	}
}
