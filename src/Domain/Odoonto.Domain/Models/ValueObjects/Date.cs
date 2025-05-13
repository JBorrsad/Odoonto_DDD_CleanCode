using System;
using System.Collections.Generic;
using Odoonto.Domain.Core.Abstractions;
using Odoonto.Domain.Core.Models.Exceptions;

namespace Odoonto.Domain.Models.ValueObjects
{
    /// <summary>
    /// Value object para representar una fecha en la zona horaria Europe/Madrid
    /// </summary>
    public class Date : ValueObject, IComparable<Date>
    {
        // TimeZoneInfo para Europe/Madrid
        private static readonly TimeZoneInfo MadridTimeZone = 
            TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time"); // "Romance Standard Time" es el ID de Windows para Europe/Madrid

        // Fecha almacenada internamente como DateTime (en UTC)
        private readonly DateTime _utcDate;

        // Propiedad pública de solo lectura para la fecha en la zona horaria Europe/Madrid
        public DateTime Value => TimeZoneInfo.ConvertTimeFromUtc(_utcDate, MadridTimeZone);

        // Propiedad de solo lectura para el día
        public int Day => Value.Day;

        // Propiedad de solo lectura para el mes
        public int Month => Value.Month;

        // Propiedad de solo lectura para el año
        public int Year => Value.Year;

        // Propiedad de solo lectura para el día de la semana
        public DayOfWeek DayOfWeek => Value.DayOfWeek;

        /// <summary>
        /// Constructor a partir de un DateTime en la zona local
        /// </summary>
        /// <param name="localDate">Fecha local</param>
        public Date(DateTime localDate)
        {
            // Convertir a fecha UTC para almacenamiento interno
            if (localDate.Kind == DateTimeKind.Unspecified)
            {
                // Si la fecha es Unspecified, asumimos que está en la zona Madrid
                _utcDate = TimeZoneInfo.ConvertTimeToUtc(
                    DateTime.SpecifyKind(localDate.Date, DateTimeKind.Unspecified), 
                    MadridTimeZone);
            }
            else
            {
                // Si la fecha ya tiene un Kind especificado, usar la conversión estándar
                _utcDate = localDate.Date.ToUniversalTime();
            }
        }

        /// <summary>
        /// Constructor a partir de año, mes y día
        /// </summary>
        public Date(int year, int month, int day)
            : this(new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Unspecified))
        {
            // Validar que la fecha sea válida
            try
            {
                // Si la fecha no es válida, DateTime constructor lanzará ArgumentOutOfRangeException
                new DateTime(year, month, day);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidValueException($"Fecha inválida: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene la fecha actual según la zona horaria Europe/Madrid
        /// </summary>
        public static Date Today()
        {
            var madridNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, MadridTimeZone);
            return new Date(madridNow.Date);
        }

        /// <summary>
        /// Suma días a la fecha
        /// </summary>
        /// <param name="days">Número de días a sumar</param>
        /// <returns>Nueva fecha con los días sumados</returns>
        public Date AddDays(int days)
        {
            return new Date(Value.AddDays(days));
        }

        /// <summary>
        /// Suma meses a la fecha
        /// </summary>
        /// <param name="months">Número de meses a sumar</param>
        /// <returns>Nueva fecha con los meses sumados</returns>
        public Date AddMonths(int months)
        {
            return new Date(Value.AddMonths(months));
        }

        /// <summary>
        /// Suma años a la fecha
        /// </summary>
        /// <param name="years">Número de años a sumar</param>
        /// <returns>Nueva fecha con los años sumados</returns>
        public Date AddYears(int years)
        {
            return new Date(Value.AddYears(years));
        }

        /// <summary>
        /// Verifica si la fecha es futura
        /// </summary>
        public bool IsFuture()
        {
            return this > Today();
        }

        /// <summary>
        /// Verifica si la fecha es pasada
        /// </summary>
        public bool IsPast()
        {
            return this < Today();
        }

        /// <summary>
        /// Calcula la edad en años a partir de esta fecha
        /// </summary>
        /// <returns>Edad en años</returns>
        public int CalculateAge()
        {
            var today = Today().Value;
            var birthDate = Value;
            
            int age = today.Year - birthDate.Year;
            
            // Ajustar si aún no ha llegado el cumpleaños de este año
            if (today.Month < birthDate.Month || 
                (today.Month == birthDate.Month && today.Day < birthDate.Day))
            {
                age--;
            }
            
            return Math.Max(0, age);
        }

        // Implementación de ValueObject
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _utcDate.Date;
        }

        // Implementación de IComparable
        public int CompareTo(Date other)
        {
            if (other is null) return 1;
            return _utcDate.Date.CompareTo(other._utcDate.Date);
        }

        // Operadores de comparación
        public static bool operator <(Date left, Date right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(Date left, Date right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(Date left, Date right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(Date left, Date right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            return left.CompareTo(right) >= 0;
        }

        // Representación en string
        public override string ToString()
        {
            return Value.ToString("yyyy-MM-dd");
        }
    }
} 