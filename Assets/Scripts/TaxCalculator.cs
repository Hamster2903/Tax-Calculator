using UnityEngine;
using SpeechLib;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class TaxCalculator : MonoBehaviour
{
    // Constant rate for the Medicare Levy
    const double MEDICARE_LEVY = 0.02;

    // Variables
    bool textToSpeechEnabled = true;
    public InputField inputGrossSalaryBox;
    public Dropdown payPeriodDropdown;
    public Text medicareLevyPaidoOutputText;
    public Text netIncomeOutputText;
    public Text incomeTaxPaidOutputText;

    private void Start()
    {
        Speak("Welcome to the A.T.O. Tax Calculator");
    }

    // Run this function on the click event of your 'Calculate' button
    public void Calculate()
    {
        // Initialisation of variables
        double medicareLevyPaid = 0;
        double incomeTaxPaid = 0;

        // Input
        double grossSalaryInput = GetGrossSalary();
        string salaryPayPeriod = GetSalaryPayPeriod();

        // Calculations
        double grossYearlySalary = CalculateGrossYearlySalary(grossSalaryInput, salaryPayPeriod);
        double netIncome = CalculateNetIncome(grossYearlySalary, ref medicareLevyPaid, ref incomeTaxPaid);

        // Output
        OutputResults(medicareLevyPaid, incomeTaxPaid, netIncome);
    }

    private double GetGrossSalary()
    {
        
        if(double.TryParse(inputGrossSalaryBox.text, out double grossSalaryInput))
        {
            return grossSalaryInput;
        }
        else
        {
            grossSalaryInput = 0;
        }
        return grossSalaryInput;
    }

    private string GetSalaryPayPeriod()
    {
        
        string salaryPayPeriod;
        if (payPeriodDropdown.value == 0)
        {
            salaryPayPeriod = "weekly";
        }
        else if (payPeriodDropdown.value == 1)
        {
            salaryPayPeriod = "fortnightly";
        }
        else if (payPeriodDropdown.value == 2)
        {
            salaryPayPeriod = "monthly";
        }
        else
        {
            salaryPayPeriod = "yearly";
        }
        
        return salaryPayPeriod;
    }

    private double CalculateGrossYearlySalary(double grossSalaryInput, string salaryPayPeriod)
    {
        
        if (salaryPayPeriod == "weekly")
        {
            return (grossSalaryInput * 52);
        }
        else if(salaryPayPeriod == "fortnightly")
        {
            return (grossSalaryInput * 26);
        }
        else if (salaryPayPeriod == "monthly")
        {
            return (grossSalaryInput * 12);
        }
        else
        {
            return grossSalaryInput;
        }
        
    }

    private double CalculateNetIncome(double grossYearlySalary, ref double medicareLevyPaid, ref double incomeTaxPaid)
    {
        
        medicareLevyPaid = CalculateMedicareLevy(grossYearlySalary);
        incomeTaxPaid = CalculateIncomeTax(grossYearlySalary);
        double netIncome = grossYearlySalary - medicareLevyPaid - incomeTaxPaid;
              
        return netIncome;
    }

    private double CalculateMedicareLevy(double grossYearlySalary)
    {
        
        double medicareLevyPaid = (grossYearlySalary * 0.02);    
        return medicareLevyPaid;
    }

    private double CalculateIncomeTax(double grossYearlySalary)
    {
        
        double taxableAmount = 0;
        if(grossYearlySalary <= 18200)
        {
            return taxableAmount;
        }
        else if (grossYearlySalary <=37000)
        {
            taxableAmount = grossYearlySalary - 18200;
            return taxableAmount * 0.19;
        }
        else if(grossYearlySalary<=87000)
        {
            taxableAmount = grossYearlySalary - 37000;
            return (taxableAmount * 0.325 + 3572);
        }
        else if(grossYearlySalary<=180000)
        {
            taxableAmount = grossYearlySalary - 87000;
            return (taxableAmount * 0.37 + 19822);
        }
        else
        {
            taxableAmount = grossYearlySalary - 180000;
            return (taxableAmount * 0.45 + 54232);
        }
       
    }

    private void OutputResults(double medicareLevyPaid, double incomeTaxPaid, double netIncome)
    {
        medicareLevyPaidoOutputText.text = medicareLevyPaid.ToString("C2");
        netIncomeOutputText.text = netIncome.ToString("C2");
        incomeTaxPaidOutputText.text = incomeTaxPaid.ToString("C2");
    }

    // Text to Speech
    private void Speak(string textToSpeak)
    {
        if(textToSpeechEnabled)
        {
            SpVoice voice = new SpVoice();
            voice.Speak(textToSpeak);
        }
    }
}
