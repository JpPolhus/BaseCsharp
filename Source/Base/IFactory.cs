﻿namespace Base
{
	public interface IFactory<out T>
	{
		T Create();
	}
}
