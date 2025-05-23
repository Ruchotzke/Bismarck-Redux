using System;

namespace bismarck_redux.population
{
    /// <summary>
    /// A representation of a single unit of population.
    /// </summary>
    public struct Pop
    {
        /// <summary>
        ///  THe number of pops in this group.
        /// </summary>
        public readonly double Size;

        /// <summary>
        /// The profession of this group of pops.
        /// </summary>
        public readonly string Profession;

        /// <summary>
        /// Hash this pop, using everything but the size.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Profession);
        }
    } 
}
