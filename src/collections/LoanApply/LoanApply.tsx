import { useState } from "react";
import { Button, MenuItem, TextField, Box } from "@mui/material";
// import MDButton from "../../components/MDButton";

import "./LoanApply.css"; // Importing the CSS file

function LoanApply() {
  const [selectedLoanType, setSelectedLoanType] = useState("");
  const [selectedMonths, setSelectedMonths] = useState("");
  const [loanAmount, setLoanAmount] = useState("");
  const [selectedCurrency, setSelectedCurrency] = useState("");
  const [monthlyInstallment, setMonthlyInstallment] = useState(null);
  const [falseData, setFalseData] = useState(false);

  const currencies = [
    { value: "USD", label: "$" },
    { value: "EUR", label: "€" },
    { value: "BGN", label: "lev" },
  ];

  const monthRange = {
    personal: { min: 3, max: 120 },
    mortgage: { min: 12, max: 420 },
    student: { min: 3, max: 60 },
  };

  const amountRange = {
    personal: { min: 500, max: 75000 },
    mortgage: { min: 10000, max: 1000000 },
    student: { min: 1000, max: 10000 },
  };

  const interestRates = {
    personal: 5.38 / 100,
    mortgage: 2.79 / 100,
    student: 7 / 100,
  };

  const handleLoanTypeChange = (event) => {
    setSelectedLoanType(event.target.value);
  };

  const handleMonthChange = (event) => {
    setSelectedMonths(event.target.value);
  };

  const handleAmountChange = (event) => {
    setLoanAmount(event.target.value);
  };

  const calculateMonthlyInstallment = (amount, months) => {
    const principal = parseFloat(amount);
    const numPayments = parseInt(months);

    // Input validation
    if (
      !selectedLoanType ||
      isNaN(principal) ||
      isNaN(numPayments) ||
      principal < amountRange[selectedLoanType].min ||
      principal > amountRange[selectedLoanType].max ||
      numPayments < monthRange[selectedLoanType].min ||
      numPayments > monthRange[selectedLoanType].max
    ) {
      setMonthlyInstallment(null);
      setFalseData(true);
      return false;
    }
    setFalseData(false);

    const interestRate = interestRates[selectedLoanType] / 12; // Monthly interest rate

    const numerator =
      principal * interestRate * Math.pow(1 + interestRate, numPayments);
    const denominator = Math.pow(1 + interestRate, numPayments) - 1;

    const installment = numerator / denominator;

    setMonthlyInstallment(installment.toFixed(2));
  };

  const handleCalculateClick = () => {
    calculateMonthlyInstallment(loanAmount, selectedMonths); // Pass amount and months when the button is clicked
  };

  return (
    <Box component="form" className="loan-apply-form">
      <h1>Apply for loan</h1>
      <div className="input-calc">
        <div className="input">
          <TextField
            id="outlined-basic"
            label="Name"
            variant="outlined"
            helperText="Enter your full name"
          />

          <TextField
            id="outlined-select-currency"
            select
            label="Currency"
            value={selectedCurrency}
            onChange={(event) => setSelectedCurrency(event.target.value)}
            helperText="Please select your currency"
          >
            {currencies.map((option) => (
              <MenuItem key={option.value} value={option.value}>
                {option.label}
              </MenuItem>
            ))}
          </TextField>

          <TextField
            id="outlined-select-type"
            select
            value={selectedLoanType}
            label="Loan Type"
            onChange={handleLoanTypeChange}
            helperText="Please select your loan type"
          >
            <MenuItem value="personal">Personal Loan</MenuItem>
            <MenuItem value="mortgage">Mortgage Loan</MenuItem>
            <MenuItem value="student">Student Loan</MenuItem>
          </TextField>

          {selectedLoanType && (
            <TextField
              label="Loan Amount"
              type="number"
              value={loanAmount}
              onChange={handleAmountChange}
              slotProps={{
                input: {
                  inputProps: {
                    min: amountRange[selectedLoanType]?.min,
                    max: amountRange[selectedLoanType]?.max,
                  },
                },
              }}
              helperText={`Enter an amount between ${amountRange[selectedLoanType]?.min} and ${amountRange[selectedLoanType]?.max}`}
            />
          )}

          {selectedLoanType && (
            <TextField
              label="Select Months"
              type="number"
              value={selectedMonths}
              onChange={handleMonthChange}
              slotProps={{
                input: {
                  inputProps: {
                    min: monthRange[selectedLoanType]?.min,
                    max: monthRange[selectedLoanType]?.max,
                  },
                },
              }}
              helperText={`Choose between ${monthRange[selectedLoanType]?.min} and ${monthRange[selectedLoanType]?.max} months`}
            />
          )}
        </div>
        <div className="calc-installment">
          <Button onClick={handleCalculateClick}>
            Calculate Monthly Installment
          </Button>
          {monthlyInstallment !== null && (
            <div>
              <h3 style={{ justifyContent: "center", display: "flex" }}>
                Monthly Installment: {monthlyInstallment}{" "}
                {
                  currencies.find((curr) => curr.value === selectedCurrency)
                    ?.label
                }
              </h3>
            </div>
          )}

          {falseData && (
            <h3
              style={{
                color: "red",
                justifyContent: "center",
                display: "flex",
              }}
            >
              Input data incorrect
            </h3>
          )}
        </div>
      </div>
      <div style={{ justifyContent: "center", display: "flex" }}>
        <Button variant="contained" type="submit">
          Apply
        </Button>
      </div>
    </Box>
  );
}

export default LoanApply;
