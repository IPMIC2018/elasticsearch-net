using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Nest
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	[JsonConverter(typeof(ReadAsTypeJsonConverter<HistogramAggregator>))]
	public interface IHistogramAggregator : IBucketAggregator
	{
		[JsonProperty("field")]
		Field Field { get; set; }

		[JsonProperty("script")]
		string Script { get; set; }

		[JsonProperty("params")]
		FluentDictionary<string, object> Params { get; set; }

		[JsonProperty("interval")]
		double? Interval { get; set; }

		[JsonProperty("min_doc_count")]
		int? MinimumDocumentCount { get; set; }

		[JsonProperty("order")]
		HistogramOrder Order { get; set; }

		[JsonProperty("extended_bounds")]
		ExtendedBounds<double> ExtendedBounds { get; set; }

		[JsonProperty("pre_offset")]
		long? PreOffset { get; set; }

		[JsonProperty("post_offset")]
		long? PostOffset { get; set; }
	}

	public class HistogramAggregator : BucketAggregator, IHistogramAggregator
	{
		public Field Field { get; set; }
		public string Script { get; set; }
		public FluentDictionary<string, object> Params { get; set; }
		public double? Interval { get; set; }
		public int? MinimumDocumentCount { get; set; }
		public HistogramOrder Order { get; set; }
		public ExtendedBounds<double> ExtendedBounds { get; set; }
		public long? PreOffset { get; set; }
		public long? PostOffset { get; set; }
	}

	public class HistogramAgg : BucketAgg, IHistogramAggregator
	{
		public Field Field { get; set; }
		public string Script { get; set; }
		public FluentDictionary<string, object> Params { get; set; }
		public double? Interval { get; set; }
		public int? MinimumDocumentCount { get; set; }
		public HistogramOrder Order { get; set; }
		public ExtendedBounds<double> ExtendedBounds { get; set; }
		public long? PreOffset { get; set; }
		public long? PostOffset { get; set; }

		public HistogramAgg(string name) : base(name) { }

		internal override void WrapInContainer(AggregationContainer c) => c.Histogram = this;
	}

	public class HistogramAggregatorDescriptor<T>
		: BucketAggregatorBaseDescriptor<HistogramAggregatorDescriptor<T>, IHistogramAggregator, T>, IHistogramAggregator
		where T : class
	{
		Field IHistogramAggregator.Field { get; set; }

		string IHistogramAggregator.Script { get; set; }

		FluentDictionary<string, object> IHistogramAggregator.Params { get; set; }

		double? IHistogramAggregator.Interval { get; set; }

		int? IHistogramAggregator.MinimumDocumentCount { get; set; }

		HistogramOrder IHistogramAggregator.Order { get; set; }

		ExtendedBounds<double> IHistogramAggregator.ExtendedBounds { get; set; }

		long? IHistogramAggregator.PreOffset { get; set; }

		long? IHistogramAggregator.PostOffset { get; set; }

		public HistogramAggregatorDescriptor<T> Field(string field) => Assign(a => a.Field = field);

		public HistogramAggregatorDescriptor<T> Field(Expression<Func<T, object>> field) => Assign(a => a.Field = field);

		public HistogramAggregatorDescriptor<T> Script(string script) => Assign(a => a.Script = script);

		public HistogramAggregatorDescriptor<T> Params(Func<FluentDictionary<string, object>, FluentDictionary<string, object>> paramSelector) =>
			Assign(a => a.Params = paramSelector?.Invoke(new FluentDictionary<string, object>()));

		public HistogramAggregatorDescriptor<T> Interval(double interval) => Assign(a => a.Interval = interval);

		public HistogramAggregatorDescriptor<T> MinimumDocumentCount(int minimumDocumentCount) =>
			Assign(a => a.MinimumDocumentCount = minimumDocumentCount);

		public HistogramAggregatorDescriptor<T> Order(HistogramOrder order) => Assign(a => a.Order = order);

		public HistogramAggregatorDescriptor<T> OrderAscending(string key) =>
			Assign(a => a.Order = new HistogramOrder { Key = key, Order = SortOrder.Descending });

		public HistogramAggregatorDescriptor<T> OrderDescending(string key) =>
			Assign(a => a.Order = new HistogramOrder { Key = key, Order = SortOrder.Descending });

		public HistogramAggregatorDescriptor<T> ExtendedBounds(double min, double max) =>
			Assign(a => a.ExtendedBounds = new ExtendedBounds<double> { Minimum = min, Maximum = max });

		public HistogramAggregatorDescriptor<T> PreOffset(long preOffset) => Assign(a => a.PreOffset = preOffset);

		public HistogramAggregatorDescriptor<T> PostOffset(long postOffset) => Assign(a => a.PostOffset = postOffset);
	}
}