﻿using System;
using System.Threading.Tasks;

namespace Actor.Base
{
    public interface IFuture : IActor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        Task<object> GetResultAsync();
    }

    public interface IFuture<T> : IFuture
    {
        T Result();
        T Result(int timeOutMS);
        Task<T> ResultAsync();
        Task<T> ResultAsync(int timeOutMS);
        T Result(Func<object, bool> aPattern);
        Task<T> ResultAsync(Func<object, bool> aPattern);
        Task<T> ResultAsync(Func<object, bool> aPattern, int timeOutMS);
    }

    public interface IFuture<T1, T2> : IFuture
    {
        IMessageParam<T1, T2> Result();
        IMessageParam<T1, T2> Result(int timeOutMS);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        Task<IMessageParam<T1, T2>> ResultAsync();
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        Task<IMessageParam<T1, T2>> ResultAsync(int timeOutMS);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public interface IFuture<T1, T2, T3> : IFuture
    {
        IMessageParam<T1, T2, T3> Result();
        IMessageParam<T1, T2, T3> Result(int timeOutMS);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        Task<IMessageParam<T1, T2, T3>> ResultAsync();
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        Task<IMessageParam<T1, T2, T3>> ResultAsync(int timeOutMS);
    }
}