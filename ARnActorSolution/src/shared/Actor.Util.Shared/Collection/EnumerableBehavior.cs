﻿using System;
using System.Collections.Generic;
using Actor.Base;
using System.Collections;

namespace Actor.Util
{
    public static class QueryActor
    {
        public static EnumerableActor<T> AsActorQueryiable<T>(this IEnumerable<T> source)
        {
            return new EnumerableActor<T>(source) ;
        }
    }

    public class EnumerableActor<T> : BaseActor, IEnumerable<T>, IEnumerable, ICollection<T>
    {
        private readonly List<T> _list = new List<T>();

        public int Count
        {
            get
            {
                var future = new Future<int>();
                (this).SendMessage((Action<IActor>)((a) => a.SendMessage(_list.Count)), (IActor)future);
                return future.Result();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public EnumerableActor() : base() => SetBehavior();

        public EnumerableActor(IEnumerable<T> source) : base()
        {
            Become(new Behavior<IEnumerable<T>>(SetupData));
            SendMessage(source);
        }

        private void SetupData(IEnumerable<T> source)
        {
            _list.AddRange(source);
            SetBehavior();
        }

        private void SetBehavior()
        {
            Become(new Behavior<Action<T>, T>((a, t) => a(t)));
            AddBehavior(new Behavior<Action<IActor>, IActor>((a, i) => a(i)));
            AddBehavior(new Behavior<Action<IActor, T>, IActor, T>((a, i, t) => a(i, t)));
            AddBehavior(new Behavior<Action>((a) => a()));
        }

        public IEnumerator<T> GetEnumerator() => new ActorEnumerator<T>(this);

        IEnumerator IEnumerable.GetEnumerator() => new ActorEnumerator<T>(this);

        public void Add(T item) => this.SendMessage<Action<T>, T>(t => _list.Add(t), item);

        public void Clear() => SendMessage((Action)(() => _list.Clear()));

        public bool Contains(T item)
        {
            var future = new Future<bool>();
            (this).SendMessage((Action<IActor, T>)((a, t) => a.SendMessage(_list.Contains(t))), (IActor)future, item);
            return future.Result();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.SendMessage<Action<T[], int>, T[], int>(
                (tab, i) => _list.CopyTo(tab, i),
                array,
                arrayIndex) ;
        }

        public bool Remove(T item)
        {
            var future = new Future<bool>();
            (this).SendMessage((Action<IActor, T>)((a, t) => a.SendMessage(_list.Remove(t))), (IActor)future, item);
            return future.Result();
        }

        private class ActorEnumerator<TSource> : BaseActor, IEnumerator<TSource>, IEnumerator, IDisposable
        {
            private readonly EnumerableActor<TSource> fCollection;
            private enum EnumeratorAction { MoveNext,Reset, Current};
            private int fIndex;

            public ActorEnumerator(EnumerableActor<TSource> aCollection) : base()
            {
                fCollection = aCollection;
                fIndex = -1;
                Become(new ActionBehavior<IActor>());
            }

            public bool MoveNext()
            {
                var future = new Future<bool>();
                fCollection.SendMessage<Action<IActor>, IActor>((a) =>
                   {
                       fIndex++;
                       a.SendMessage(fIndex < fCollection._list.Count);
                   }, future) ;
                return future.Result();
            }

            // better than this ?
            public void Reset()
            {
                var future = new Future<int>();
                this.SendMessage<Action<IActor>, IActor>((a) =>
                {
                    fIndex = -1;
                    a.SendMessage(fIndex);
                }, future);
                future.Result();
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposable)
            {
                if (disposable)
                {
                }
            }

            TSource IEnumerator<TSource>.Current
            {
                get {
                    var future = new Future<TSource>();
                    fCollection.SendMessage((Action<IActor>)((a) => a.SendMessage(fCollection._list[fIndex])), (IActor)future);
                    return future.Result();
                }
            }

            object IEnumerator.Current
            {
                get {
                    var future = new Future<TSource>();
                    fCollection.SendMessage((Action<IActor>)((a) => a.SendMessage(fCollection._list[fIndex])), (IActor)future);
                    return future.Result();
                }
            }
        }
    }
}
