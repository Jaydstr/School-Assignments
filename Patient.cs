//Group members
//Jaida Mendez – 0698734
//Amy Paterson - 0641278

using hospital_emergency_room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hospital_emergency_room
{
    public class Patient                                    //patient class, build the components of a patient
    {
        private static Random random = new Random();        //create a random number stored in random

                                                            //properties for each patient
        public int PatientNumber { get; }                   //property patient number
        public int EmergencyLevel { get; }                  //property emergency level
        public double TreatmentTime { get; }                //property treatment time
        public int ArrivalTime { get; }                     //property arrival time
                     
        public Patient(int patientNumber, double meanTreatmentTime, int arrivalTime)//construsctor for patient
        {
            PatientNumber = patientNumber;                                          //patient number
            TreatmentTime = GenerateTreatmentTime(meanTreatmentTime);               //how long treatment time will take                      
            ArrivalTime = arrivalTime;                                              //the random time of arival for each patient
            EmergencyLevel = GenerateEmergencyLevel();                              //emergency level assigned to each patient 1 to 3 
                       
        }   

        private int GenerateEmergencyLevel()            //creates the emergency level for the patient based on a probablity below
        {
            
            double randomNumber = random.NextDouble();  //generates  random number between 0 and 1

            if (randomNumber < 0.6)                     //if random number is less than 0.6 which will happen 60 % of the time

            {
                return 1;                               //assign level 1
            }

            else if (randomNumber < 0.9)                //if random number is less than 0.9 but higher than0.6 which will happen 30% of the time
            {
                return 2;                               //assign level 2
            }

            else
            {
                return 3;                               //assign level 3 if the number is higher than 0.9 which will only happen 10% of the time
            }
        }


        private double GenerateTreatmentTime(double meanTreatmentTime)  //method that creates the random treatment time per a patient
        {
            double u = random.NextDouble();                             //creates the random number and stores it in the double u
            return -meanTreatmentTime * Math.Log(u);                    //retuns the positive random number
        }
    }
}




