//Group members
//Jaida Mendez – 0698734
//Amy Paterson - 0641278

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace hospital_emergency_room
{
    public class Event : IComparable<Event>     //class event uses IComparable type Event
    {
        public enum EventType                   // event types
        {
            ARRIVAL,
            DEPARTURE
        }
                                                //properties
        public Patient Patient { get; }         //property patient
        public EventType Type { get; }          //property event type
        public int Doctor { get; set; }         //property doctor
        public int Time { get; }                //property time

        public Event(Patient patient, EventType type, int doctor, int time) //constructor for arival event, takes 4 parameters
        {
            Patient = patient;
            Type = type;
            Doctor = doctor;
            Time = time;
        }

        public Event(Patient patient, EventType type, int time)             //constructor for departure event, takes 3 parameters
        {
            Patient = patient;
            Type = type;
            Time = time;
        }

        public int CompareTo(Event otherEvent)                              //compare to method
        {
            return Time.CompareTo(otherEvent.Time);                         //compares the time of one event to another and returns it
        }
    }

}
