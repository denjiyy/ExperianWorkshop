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
import "./style.css";

function LoanListAdmin() {
  const [loans, setLoans] = useState([]);
  const [filteredLoans, setFilteredLoans] = useState([]);
  const [startDate, setStartDate] = useState<Dayjs | null>(null);
  const [endDate, setEndDate] = useState<Dayjs | null>(null);
  const [error, setError] = useState("");

  useEffect(() => {
    setLoans([
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

  useEffect(() => {
    const filterLoans = () => {
      const filtered = loans.filter((loan) => {
        const loanDate = dayjs(loan.date * 1000); // Ensure the timestamp is in milliseconds

        if (startDate && loanDate.isBefore(startDate)) {
          return false;
        }

        if (endDate && loanDate.isAfter(endDate)) {
          return false;
        }

        return true;
      });

      setFilteredLoans(filtered);
    };

    if (loans.length > 0) {
      filterLoans();
    }
  }, [startDate, endDate, loans]);

  const handleStartDateChange = (newValue) => {
    setStartDate(newValue);
    if (endDate && newValue && dayjs(newValue).isAfter(endDate)) {
      setEndDate(null); // Reset end date if it becomes invalid
      setError("End date should be after the start date");
    } else {
      setError("");
    }
  };

  const handleEndDateChange = (newValue) => {
    if (startDate && newValue && dayjs(newValue).isBefore(startDate)) {
      setError("End date should be after the start date");
    } else {
      setEndDate(newValue);
      setError("");
    }
  };

  return (
    <div className="loans">
      <h1>Loans to approve</h1>
      <div className="table">
        <div className="date-picker">
          <LocalizationProvider dateAdapter={AdapterDayjs}>
            <DemoContainer components={["DatePicker"]}>
              <DatePicker
                label="Start Date"
                value={startDate}
                onChange={handleStartDateChange}
              />
            </DemoContainer>
          </LocalizationProvider>

          <LocalizationProvider dateAdapter={AdapterDayjs}>
            <DemoContainer components={["DatePicker"]}>
              <DatePicker
                label="End Date"
                value={endDate}
                minDate={startDate} // Ensure end date is after start date
                onChange={handleEndDateChange}
              />
            </DemoContainer>
          </LocalizationProvider>
        </div>

        {error && <p style={{ color: "red" }}>{error}</p>}
        <TableContainer
          className="table"
          sx={{
            maxWidth: "1000px",
          }}
          // pageSizeOptions={[5, 10]}
          component={Paper}
        >
          <Table sx={{ minWidth: 650 }} aria-label="simple table">
            <TableHead>
              <TableRow>
                <TableCell align="right">Date</TableCell>
                <TableCell align="right">Owner</TableCell>
                <TableCell align="right">Type</TableCell>
                <TableCell align="right">Amount</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {filteredLoans.map((loan, index) => (
                <TableRow
                  key={index}
                  sx={{ "&:last-child td, &:last-child th": { border: 0 } }}
                >
                  <TableCell align="right">
                    {dayjs(loan.date * 1000).format("DD/MM/YYYY")}
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
    </div>
  );
}

export default LoanListAdmin;
