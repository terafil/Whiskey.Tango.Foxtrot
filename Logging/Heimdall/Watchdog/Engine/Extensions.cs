using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Watchdog.Engine
{
    public static class Extensions
    {
        private static XElement CreateXElement(string name, object value = null)
        {
            XElement element = new XElement(XName.Get(name));
            if (value != null)
            {
                element.Value = value.ToString();
            }
            return element;
        }
        public static XElement ToXElement(this ApplicationLogEntry entry)
        {
            XElement entryElement = CreateXElement("entry");
            entryElement.Add(CreateXElement("occurredOn", entry.OccurredOn));
            entryElement.Add(CreateXElement("level", entry.LogLevel));
            entryElement.Add(CreateXElement("message", entry.Message));
            return entryElement;
        }
    }
}