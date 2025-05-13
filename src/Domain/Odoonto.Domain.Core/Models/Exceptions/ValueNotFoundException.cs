using System;

namespace Odoonto.Domain.Core.Models.Exceptions
{
    /// <summary>
    /// Excepción que se lanza cuando un valor no se encuentra en el repositorio
    /// </summary>
    public class ValueNotFoundException : Exception
    {
        public ValueNotFoundException() 
            : base("El valor solicitado no fue encontrado.")
        {
        }

        public ValueNotFoundException(string message) 
            : base(message)
        {
        }

        public ValueNotFoundException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
} 