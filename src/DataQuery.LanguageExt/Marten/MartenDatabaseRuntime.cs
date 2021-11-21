using System;
using System.Threading;
using LanguageExt;
using Marten;

namespace DataQuery.LanguageExt.Marten
{
    using static Prelude;

    public static partial class DataQueryMarten
    {
        /// <summary>
        /// Default database runtime, cancellable, allows access to the
        /// current Marten's session
        /// </summary>
        public readonly struct MartenDatabaseRuntime : HasMartenDatabase<MartenDatabaseRuntime>
        {
            private MartenDatabaseRuntime(MartenDatabaseRuntimeEnv env) => Env = env;

            public static MartenDatabaseRuntime New(IDocumentSession session, CancellationToken cancelToken) =>
                new(new MartenDatabaseRuntimeEnv(new CancellationTokenSource(), cancelToken, session));

            public MartenDatabaseRuntimeEnv Env { get; }
            public MartenDatabaseRuntime LocalCancel => new(Env.LocalCancel);
            public CancellationToken CancellationToken => Env.Token;
            public CancellationTokenSource CancellationTokenSource => Env.Source;
            public IDocumentSession Session => Env.Session;

            public Eff<MartenDatabaseRuntime, IMartenDatabaseIO> MartenDatabaseEff =>
                SuccessEff(LiveMartenDatabaseIO.Default);
        }

        public readonly struct MartenDatabaseRuntimeEnv
        {
            public readonly CancellationTokenSource Source;
            public readonly CancellationToken Token;
            public readonly IDocumentSession Session;

            public MartenDatabaseRuntimeEnv(
                CancellationTokenSource source,
                CancellationToken token,
                IDocumentSession session)
            {
                Source = source;
                Token = token;
                Session = session;
            }

            private MartenDatabaseRuntimeEnv(CancellationTokenSource source, IDocumentSession session)
                : this(source, source.Token, session)
            { }

            public MartenDatabaseRuntimeEnv LocalCancel => new(new CancellationTokenSource(), Session);
        }
    }
}