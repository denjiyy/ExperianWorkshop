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

function Transactions() {
  const [transactions, setTransactions] = useState([]);
  const [filteredTransactions, setFilteredTransactions] = useState([]);
  const [startDate, setStartDate] = useState<Dayjs | null>(null);
  const [endDate, setEndDate] = useState<Dayjs | null>(null);
  useEffect(() => {
    setTransactions([
      {
        id: 0,
        currency: "BGN",
        accountId: 5,
        status: "pending",
        type: "uu",
        amount: 2000,
        date: 1413561532,
        description: "",
      },
      {
        id: 1,
        currency: "BGN",
        accountId: 5,
        status: "nnn",
        type: "uu",
        amount: 2200,
        date: 1413561532,
        description: "",
      },
    ]);
  }, []);

  // // Fetch transactions once when the component mounts
  // useEffect(() => {
  //   axios.get('http://localhost:5000/api/transactions')
  //     .then(response => {
  //       setTransactions(response.data);
  //       setFilteredTransactions(response.data);  // Initially show all transactions
  //     })
  //     .catch(error => {
  //       console.error('Error fetching transactions:', error);
  //     });
  // }, []);

  useEffect(() => {
    const filterTransactions = () => {
      const filtered = transactions.filter((transaction) => {
        const transactionDate = dayjs(transaction.date); // Convert to Dayjs object

        if (startDate && transactionDate.isBefore(dayjs(startDate))) {
          return false;
        }

        if (endDate && transactionDate.isAfter(dayjs(endDate))) {
          return false;
        }

        return true;
      });
      setFilteredTransactions(filtered);
    };

    // Call the function only if transactions are fetched
    if (transactions.length > 0) {
      filterTransactions();
    }
  }, [startDate, endDate, transactions, filteredTransactions]);

  return (
    <div className="Transactions">
      <h1>Previous Transactions</h1>
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
              <TableCell>Transactions</TableCell>
              <TableCell align="right">Description</TableCell>
              <TableCell align="right">Currency</TableCell>
              <TableCell align="right">Date</TableCell>
              <TableCell align="right">Amount</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {filteredTransactions.map((transaction) => (
              <TableRow
                key={transaction.id}
                sx={{ "&:last-child td, &:last-child th": { border: 0 } }}
              >
                <TableCell component="th" scope="row">
                  {transaction.id}
                </TableCell>
                <TableCell align="right">{transaction.description}</TableCell>
                <TableCell align="right">{transaction.currency}</TableCell>
                <TableCell align="right">
                  {new Date(transaction.date).toLocaleDateString()}
                </TableCell>
                <TableCell align="right">{transaction.amount}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </div>
  );
}

export default Transactions;
