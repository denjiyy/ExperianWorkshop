import React, { useState, useEffect } from "react";
import "react-datepicker/dist/react-datepicker.css";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import dayjs from "dayjs";
import { Dayjs } from "dayjs";
import { DemoContainer } from "@mui/x-date-pickers/internals/demo";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";

function LoanListAdmin() {
  const [loans, setTransactions] = useState([]);
  const [filteredLoans, setFilteredLoaans] = useState([]);
  const [startDate, setStartDate] = useState<Dayjs | null>(null);
  const [endDate, setEndDate] = useState<Dayjs | null>(null);
  useEffect(() => {
    setTransactions([
      {
        date: 1413561532,
        owner: "Gosho",
        type: "personal",
        amount: 2000,
      },
      {
        date: 1413561532,
        owner: "Pesho",
        type: "personal",
        amount: 25000,
      },
    ]);
  }, []);

  // // Fetch loans once when the component mounts
  // useEffect(() => {
  //   axios.get('http://localhost:5000/api/loans')
  //     .then(response => {
  //       setTransactions(response.data);
  //       setFilteredTransactions(response.data);  // Initially show all loans
  //     })
  //     .catch(error => {
  //       console.error('Error fetching loans:', error);
  //     });
  // }, []);

  useEffect(() => {
    const filterLoans = () => {
      const filtered = loans.filter((transaction) => {
        const transactionDate = dayjs(transaction.date); // Convert to Dayjs object

        if (startDate && transactionDate.isBefore(dayjs(startDate))) {
          return false;
        }

        if (endDate && transactionDate.isAfter(dayjs(endDate))) {
          return false;
        }

        return true;
      });
      setFilteredLoaans(filtered);
    };

    // Call the function only if loans are fetched
    if (loans.length > 0) {
      filterLoans();
    }
  }, [startDate, endDate, loans, filteredLoans]);

  return (
    <div className="loans">
      <h1>Loans to approve</h1>
      <div className="date-picker">
        <LocalizationProvider dateAdapter={AdapterDayjs}>
          <DemoContainer components={["DatePicker"]}>
            <DatePicker
              value={startDate}
              onChange={(newValue) => setStartDate(newValue)}
            />
          </DemoContainer>
        </LocalizationProvider>

        <LocalizationProvider dateAdapter={AdapterDayjs}>
          <DemoContainer components={["DatePicker"]}>
            <DatePicker
              value={endDate}
              onChange={(newValue) => setEndDate(newValue)}
            />
          </DemoContainer>
        </LocalizationProvider>
      </div>

      <TableContainer component={Paper}>
        <Table sx={{ minWidth: 650 }} aria-label="simple table">
          <TableHead>
            <TableRow>
              <TableCell>Loans</TableCell>
              <TableCell align="right">Date</TableCell>
              <TableCell align="right">Owner</TableCell>
              <TableCell align="right">Type</TableCell>
              <TableCell align="right">Amount</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {filteredLoans.map((loan) => (
              <TableRow
                key={loan.id}
                sx={{ "&:last-child td, &:last-child th": { border: 0 } }}
              >
                <TableCell component="th" scope="row">
                  {loan.id}
                </TableCell>
                <TableCell align="right">
                  {new Date(loan.date).toLocaleDateString()}
                </TableCell>
                <TableCell align="right">{loan.owner}</TableCell>
                <TableCell align="right">{loan.type}</TableCell>
                <TableCell align="right">{loan.amount}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </div>
  );
}

export default LoanListAdmin;
