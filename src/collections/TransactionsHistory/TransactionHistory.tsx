// import * as S from "./elements";
import React, { useState, useEffect } from "react";
// import axios from 'axios';
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

function Transactions() {
  const [transactions, setTransactions] = useState([]);
  const [filteredTransactions, setFilteredTransactions] = useState([]);
  const [startDate, setStartDate] = useState(null);
  const [endDate, setEndDate] = useState(null);
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
        const transactionDate = new Date(transaction.date);

        if (startDate && transactionDate < startDate) {
          return false;
        }

        if (endDate && transactionDate > endDate) {
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
        <label>Start Date: </label>
        <DatePicker
          selectsRange // Enable range selection
          startDate={startDate} // Set the start date
          endDate={endDate} // Set the end date
          onChange={(dates) => {
            const [start, end] = dates; // Destructure start and end dates
            setStartDate(start); // Update the start date state
            setEndDate(end); // Update the end date state
          }}
          dateFormat="yyyy-MM-dd" // Set the date format
          isClearable // Allow clearing the selection
        />

        <label>End Date: </label>
        <DatePicker
          selectsRange // Enable range selection
          startDate={startDate} // Set the start date
          endDate={endDate} // Set the end date
          onChange={(dates) => {
            const [start, end] = dates; // Destructure start and end dates
            setStartDate(start); // Update the start date state
            setEndDate(end); // Update the end date state
          }}
          dateFormat="yyyy-MM-dd" // Set the date format
          isClearable // Allow clearing the selection
        />
      </div>
      <table>
        <thead>
          <tr>
            <th>Description</th>
            <th>Currency</th>
            <th>Date</th>
            <th>Amount</th>
            {/* <th>Account Number</th> */}
          </tr>
        </thead>
        <tbody>
          {filteredTransactions.map((transaction) => (
            <tr key={transaction.id}>
              <td>{transaction.description}</td>
              <td>{transaction.currency}</td>
              <td>{new Date(transaction.date).toLocaleDateString()}</td>
              <td>{transaction.amount}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default Transactions;
