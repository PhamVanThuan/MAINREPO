namespace SAHL.Core.Caching
{
    public abstract class AbstractCacheKeyGeneratorFactory<T> : ICacheKeyGeneratorFactory<T>
    {
        private IHashGenerator hashGenerator;

        public AbstractCacheKeyGeneratorFactory(IHashGenerator hashGenerator)
        {
            this.hashGenerator = hashGenerator;
        }

        public string GetKey<U>(T context)
        {
            return this.hashGenerator.GenerateHash(string.Format("{0}_{1}", this.GetContextString(context).ToLower(), typeof(U).FullName.ToLower()));
        }

        protected abstract string GetContextString(T context);
    }
}