//Group members
//Jaida Mendez – 0698734
//Amy Paterson - 0641278

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace hospital_emergency_room
{
    public class Simulation     //class simultion, this class is the heart of the program
    {
        private PriorityQueue<Event> eventQueue = new PriorityQueue<Event>();   //create the priority queue
        private CustomQueue<Event>[] waitingQueues = new CustomQueue<Event>[3]; //create waiting queues for each level of patient
        private bool[] doctorAvailability;                                      // bool for is the doctor avaliable
        private int N;                                                          // variable for doctor willbeused and name will be changed later
        private int maxQueueSize = int.MaxValue;                                //the max queue size
        private int currentTime = 0;                                            //the current time starts at 0
        private Random random = new Random();                                   //random varibale

        public Simulation(int N)                //set up the simulation
        {
            this.N = N;
            doctorAvailability = new bool[N];
            for (int i = 0; i < N; i++)
            {
                doctorAvailability[i] = true;   //make all doctors initially available
            }

            for (int i = 0; i < 3; i++)
            {
                waitingQueues[i] = new CustomQueue<Event>();
            }
        }

        public void RunSimulation(int T, int M) //the run simulation method is wherer all the work is happening
        {
            int patientNumber = 1;
            int doctorNumber = 0;

            try
            {
                while (currentTime < T)     //while current timeis less than T do the following
                {
                    double meanTreatmentTime = GetRandomMeanTreatmentTime();    //get the random treatment timw for the patient
                    Patient newPatient = new Patient(patientNumber++, meanTreatmentTime, currentTime);

                    int availableDoctor = GetAvailableDoctor(); //find an available doctor
                    if (availableDoctor != -1)
                    {
                        AssignPatientToDoctor(newPatient, availableDoctor);
                    }
                    else
                    {
                        HandleWaitingRoom(newPatient);          //if the patient is comming from the waiting room
                    }

                    
                    currentTime += GetRandomInterArrivalTime(M);//move the time forward before processing events

                    
                    ProcessEvents();                            //process events at the updated time

                    
                    doctorNumber = (doctorNumber + 1) % N;      //increment and loop back to 0 when we reach the last doctor
                }
            }
            catch (Exception ex)                                //throw expection if there is an error
            {
                Console.WriteLine("");
            }
        }

        private void ProcessEvents()
        {
            
            while (!eventQueue.Empty() && eventQueue.Front().Time <= currentTime)   //process events until the event queue is empty or until the next event is beyond the current time
            {
                Event nextEvent = eventQueue.Front();
                eventQueue.Remove();

                switch (nextEvent.Type)                     //what the program will do based on the type of event
                {
                    case Event.EventType.ARRIVAL:
                        ProcessArrivalEvent(nextEvent);     //process arrival is type is arrival
                        break;
                    case Event.EventType.DEPARTURE:
                        ProcessDepartureEvent(nextEvent);   //process departure is type is departure
                        break;
                        
                }
            }
        }


        private void ProcessArrivalEvent(Event arrivalEvent)    //method to process the arrival of a patient
        {
            Patient arrivingPatient = arrivalEvent.Patient;     //uses the arrival event on the patient
            int arrivingLevel = arrivingPatient.EmergencyLevel; //assignes the emergency level

            int availableDoctor = GetAvailableDoctor();         //gets an available doctor
            if (availableDoctor != -1)
            {
                AssignPatientToDoctor(arrivalEvent.Patient, availableDoctor);
            }
            else
            {
                HandleWaitingRoom(arrivalEvent.Patient);        //ig there is no doctor availbale, but the patient in the waitinf room
            }
        }

        private void AssignPatientToDoctor(Patient arrivingPatient, int doctor)         //this method assigns a arrivng patient to a doctor
        {
            int treatmentTime = CalculateTreatmentTime(arrivingPatient.TreatmentTime);  //calculate the treatment time of the new patient
            int departureTime = currentTime + treatmentTime;                            //calculate the departure time
            Event departureEvent = new Event(arrivingPatient, Event.EventType.DEPARTURE, doctor, departureTime);
            eventQueue.Insert(departureEvent);

            Console.WriteLine($"{ToTimeString(currentTime)} - Patient {arrivingPatient.PatientNumber} ({arrivingPatient.EmergencyLevel}) arrives and is assigned to Doctor {doctor + 1}.");

            doctorAvailability[doctor] = false;                                         //set the assigned doctor to unavailable
        }

        private void HandleWaitingRoom(Patient patient)     //method deals with the waiting rooms, puts patients in their repective waiting rooms if there is no doctor available
        {
            int arrivingLevel = patient.EmergencyLevel;

            if (arrivingLevel >= 0 && arrivingLevel <= waitingQueues.Length)
            {
                if (waitingQueues[arrivingLevel].Count < maxQueueSize)
                {
                    waitingQueues[arrivingLevel].Enqueue(new Event(patient, Event.EventType.ARRIVAL, 0, currentTime));
                    Console.WriteLine($"{ToTimeString(currentTime)} - Patient {patient.PatientNumber} ({arrivingLevel}) arrives and is seated in the waiting room.");
                }
                else
                {
                    Console.WriteLine($"{ToTimeString(currentTime)} - Patient {patient.PatientNumber} ({arrivingLevel}) arrives, but the waiting room is full.");
                }
            }
            else
            {
                Console.WriteLine($"{ToTimeString(currentTime)} - Warning: Invalid emergency level {arrivingLevel}.");
            }
        }

        private void ProcessDepartureEvent(Event departureEvent)    //method for processing a departure
        {
            int doctor = departureEvent.Doctor;
            int waitingLevel = GetHighestPriorityWaitingLevel();

            if (waitingLevel != -1)
            {
                AssignPatientToDoctorFromWaitingQueue(doctor, waitingLevel);    //uses the assigne opatient to doc from waiting room method 
            }
            else
            {
                doctorAvailability[doctor] = true;
            }

            Console.WriteLine($"{ToTimeString(currentTime)} - Doctor {doctor + 1} completes treatment of Patient {departureEvent.Patient.PatientNumber}.");
        }

        private int GetAvailableDoctor()    //method for getting an available doctor
        {
            for (int i = 0; i < N; i++)
            {
                if (doctorAvailability[i])
                {
                    return i;
                }
            }
            return -1;
        }


        private void AssignPatientToDoctorFromWaitingQueue(int doctor, int waitingLevel)    //method that assigns a doctor to a patient from the waiting room
        {
            doctor = GetAvailableDoctor();  //fin the available doctor

            if (doctor != -1)
            {
                
                Event nextPatientEvent = waitingQueues[waitingLevel].Dequeue(); //dequeue the next patient from the waiting queue
                Patient nextPatient = nextPatientEvent.Patient;

               
                for (int i = 2; i > waitingLevel; i--)  //check if there are patients in the waiting queue with higher emergency levels
                {
                    if (waitingQueues[i].Count > 0)     //higher-level patient found, enqueue the current patient back to their waiting queue
                    {
                        waitingQueues[waitingLevel].Enqueue(nextPatientEvent);
                        Console.WriteLine($"{ToTimeString(currentTime)} - Patient {nextPatient.PatientNumber} ({nextPatient.EmergencyLevel}) kicked by higher-level patient. " +
                                          $"Enqueued back to waiting queue {waitingLevel}.");
                        return;
                    }
                }

                int treatmentTime = CalculateTreatmentTime(nextPatient.TreatmentTime);  //calculate treatment time for the patient

                
                int departureTime = currentTime + treatmentTime;    //calculate departure time based on treatment time

                
                Event newDepartureEvent = new Event(nextPatient, Event.EventType.DEPARTURE, doctor, departureTime); //create a new DEPARTURE event for the patient with the correct departure time

                
                eventQueue.Insert(newDepartureEvent);   //insert the new DEPARTURE event into the event queue

                doctorAvailability[doctor] = false;     //mark the doctor as unavailable during the treatment


                Console.WriteLine($"{ToTimeString(currentTime)} - Patient {nextPatient.PatientNumber} ({nextPatient.EmergencyLevel}) is assigned to Doctor {doctor + 1}."); //print the assignment of the next patient to the doctor and the treatment time


                Console.WriteLine($"{ToTimeString(departureTime)} - Doctor {doctor + 1} completes treatment of Patient {nextPatient.PatientNumber}.");  //print the completion of the treatment for the current patient


                doctorAvailability[doctor] = true;  //mark the doctor as available again after the treatment is completed

            }
            else
            {
                
                Console.WriteLine($"{ToTimeString(currentTime)} - No available doctors. Patient remains in waiting queue {waitingLevel}."); //if no doctor is available, the patient remains in the waiting queue
            }
        }


        private int GetHighestPriorityWaitingLevel()    //find the higest priority of the waiting queue
        {
            for (int i = 2; i >= 0; i--)
            {
                if (waitingQueues[i].Count > 0)
                {
                    return i;
                }
            }
            return -1;
        }

        private double GetRandomMeanTreatmentTime() 
        {
            return random.NextDouble() * 100;                   // returns a random value between 0 and 100
        }

        private int GetRandomInterArrivalTime(int M)
        {
            return (int)(-M * Math.Log(random.NextDouble()));   // returns a random value
        }

        private int CalculateTreatmentTime(double meanTreatmentTime)
        {
            double u = random.NextDouble();
            return (int)(-meanTreatmentTime * Math.Log(u));     // returns a random value
        }

        private string ToTimeString(int seconds)                //converts the time in seconds into a printable string
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return time.ToString(@"hh\:mm\:ss");
        }
    }
}
