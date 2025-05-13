using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WF.Mimetic.Domain.Core.Models.Exceptions;

namespace WF.Mimetic.Domain.Shared
{
    public static class RouteValidator
    {
        public static Regex Regex
        {
            get
            {
                return new Regex(
                    @"^(?:" +
                        @"(?:" +                                // Grupo opcional para URL completa
                            @"(https?|ftp):\/\/" +             // protocolo
                            @"(" +
                                @"localhost|" +                 // localhost
                                @"([a-z0-9]([a-z0-9-]*[a-z0-9])?\.)+[a-z0-9-]{2,}|" +  // dominio
                                @"((\d{1,3}\.){3}\d{1,3})" +   // IP v4
                            @")" +
                            @"(:\d+)?" +                       // puerto opcional
                        @")?" +                                // Fin del grupo opcional de URL completa
                    @")" +
                    @"\/" +                                    // slash inicial obligatorio
                    @"(?:" +                                   // Inicio del grupo de path
                        @"[a-z0-9\._~%!$&'()*+,;=:@\/-]+" +   // Primera parte de la ruta
                        @"(?:\/(?!\/)" +                       // slash seguido de...
                            @"(?:" +                           // grupo para cada segmento
                                @"\{\{[\w\-\._]+\}\}|" +       // variable con doble llave
                                @"[a-z0-9\._~%!$&'()*+,;=:@\/-]+" + // o texto fijo
                            @")" +
                        @")*" +                                // pueden venir más segmentos
                    @")" +
                    @"(?:\?" +                                 // inicio query string (opcional)
                        @"[a-z0-9\._~%!$&'()*+,;=:@\/-]*" +   // caracteres permitidos antes de variable
                        @"(?:\{\{[\w\-\._]+\}\})" +           // primera variable con doble llave
                        @"[a-z0-9\._~%!$&'()*+,;=:@\/-]*" +   // caracteres permitidos después de variable
                        @"(?:&" +                             // parámetros adicionales
                            @"[a-z0-9\._~%!$&'()*+,;=:@\/-]*" +
                            @"(?:\{\{[\w\-\._]+\}\}|[a-z0-9\._~%!$&'()*+,;=:@\/-]+)" +
                            @"[a-z0-9\._~%!$&'()*+,;=:@\/-]*" +
                        @")*" +
                    @")?" +                                   // fin query string (opcional)
                    @"(?:#[-a-z0-9_]*)?$"                    // fragment
                , RegexOptions.IgnoreCase);
            }
        }


        public static bool ValidateRoute(string route)
        {
            if (string.IsNullOrWhiteSpace(route))
            {
                return false;
            }

            if (!Regex.IsMatch(route))
            {
                return false;
            }

            return true;
        }
    }
}
