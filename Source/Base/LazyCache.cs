using System;

namespace Politiken.Base
{
	public class LazyCache<T>
	{
		private readonly Func<T> _provider;
		private readonly TimeSpan _timeout;
		private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

		private T _value;
		private bool _hasValue;
		private DateTimeOffset _valueTimestamp;

		public LazyCache(Func<T> provider, TimeSpan timeout, IDateTimeOffsetProvider dateTimeOffsetProvider)
		{
			_provider = provider;
			_timeout = timeout;
			_dateTimeOffsetProvider = dateTimeOffsetProvider;

			_value = default(T);
			_hasValue = false;
			_valueTimestamp = DateTimeOffset.MinValue;
		}

		public bool HasValue
		{
			get
			{
				var elapsed = _dateTimeOffsetProvider.UtcNow.Subtract(_valueTimestamp);
				return _hasValue && elapsed <= _timeout;
			}
		}

		public T Value
		{
			get
			{
				if (!HasValue)
				{
					CreateValue();
				}
				return _value;
			}
		}

		private void CreateValue()
		{
			_value = _provider();
			_valueTimestamp = _dateTimeOffsetProvider.UtcNow;
			_hasValue = true;
		}
	}
}
