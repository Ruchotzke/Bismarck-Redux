using System.Collections.Generic;

namespace bismarck_redux.population
{
    /// <summary>
    /// A container used to ease pop management.
    /// </summary>
    public class PopContainer
    {
        /// <summary>
        /// The pops contained in this container
        /// </summary>
        private HashSet<Pop> _pops;

        public PopContainer()
        {
            _pops = new HashSet<Pop>();
        }

        /// <summary>
        /// Try and find a matching pop in this container.
        /// </summary>
        /// <param name="pop"></param>
        /// <param name="matching"></param>
        /// <returns></returns>
        public bool TryGetPop(Pop pop, out Pop matching)
        {
            if (_pops.TryGetValue(pop, out matching))
            {
                return true;
            }
            else
            {
                matching = null;
                return false;
            }
        }

        /// <summary>
        /// Insert this pop or find a match and update the size.
        /// </summary>
        /// <param name="pop"></param>
        public void InsertOrUpdatePop(Pop pop)
        {
            if (this.TryGetPop(pop, out Pop toUpdate))
            {
                toUpdate.Size += pop.Size;
            }
            else
            {
                _pops.Add(pop);
            }
        }

        /// <summary>
        /// Find a matching pop and set its population to the provided size.
        /// </summary>
        /// <param name="toMatch"></param>
        /// <param name="size"></param>
        /// <exception cref="Exception"></exception>
        public void SetPopulation(Pop toMatch, double size)
        {
            if (this.TryGetPop(toMatch, out Pop matching))
            {
                matching.Size = size;
            }
            else
            {
                throw new System.Exception("Matching population could not be found");
            }
        }

        /// <summary>
        /// Change the supplied population by the given amount. If the population
        /// is reduced below zero, it's clamped to zero.
        /// </summary>
        /// <param name="toMatch"></param>
        /// <param name="amount"></param>
        /// <exception cref="Exception"></exception>
        public void DeltaPopulation(Pop toMatch, double amount)
        {
            if (this.TryGetPop(toMatch, out Pop matching))
            {
                matching.Size += amount;
                if (matching.Size < 0) matching.Size = 0;
            }
            else
            {
                throw new System.Exception("Matching population could not be found");
            }
        }

        /// <summary>
        /// Get the total population contained in this container.
        /// </summary>
        /// <returns></returns>
        public double GetTotalPopulation()
        {  
            double total = 0;
            foreach (var pop in _pops)
            {
                total += pop.Size;
            }
            return total;
        }
    }
}