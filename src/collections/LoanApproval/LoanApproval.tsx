import React from "react";
import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableRow,
  Paper,
  Button,
} from "@mui/material";
import Grid from "@mui/material/Grid2";

function LoanApproval() {
  const loanInfo = {
    name: "John Doe",
    age: 26,
    weeks: 78,
    amount: 5000,
    score1: 85,
    score2: 92,
    type: "Personal Loan",
  };

  const handleApprove = () => {
    console.log("Loan Approved");
  };

  const handleDecline = () => {
    console.log("Loan Declined");
  };

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        height: "100vh",
      }}
    >
      <Grid container spacing={3} maxWidth="md" direction="column">
        <Grid>
          <h1>Loan Approval Details</h1>
        </Grid>

        <Grid>
          <TableContainer component={Paper}>
            <Table>
              <TableBody>
                <TableRow>
                  <TableCell>
                    <strong>Name</strong>
                  </TableCell>
                  <TableCell>{loanInfo.name}</TableCell>
                </TableRow>
                <TableRow>
                  <TableCell>
                    <strong>Age</strong>
                  </TableCell>
                  <TableCell>{loanInfo.age}</TableCell>
                </TableRow>
                <TableRow>
                  <TableCell>
                    <strong>Credit score</strong>
                  </TableCell>
                  <TableCell>{loanInfo.score1}</TableCell>
                </TableRow>
                <TableRow>
                  <TableCell>
                    <strong>Risk assesment score</strong>
                  </TableCell>
                  <TableCell>{loanInfo.score2}</TableCell>
                </TableRow>
                <TableRow>
                  <TableCell>
                    <strong>Amount</strong>
                  </TableCell>
                  <TableCell>${loanInfo.amount}</TableCell>
                </TableRow>
                <TableRow>
                  <TableCell>
                    <strong>Weeks to repay</strong>
                  </TableCell>
                  <TableCell>{loanInfo.weeks}</TableCell>
                </TableRow>

                <TableRow>
                  <TableCell>
                    <strong>Type</strong>
                  </TableCell>
                  <TableCell>{loanInfo.type}</TableCell>
                </TableRow>
              </TableBody>
            </Table>
          </TableContainer>
        </Grid>

        <Grid>
          <Grid container spacing={2} justifyContent="center">
            <Grid>
              <Button
                variant="contained"
                color="primary"
                onClick={handleApprove}
              >
                Approve
              </Button>
            </Grid>
            <Grid>
              <Button variant="contained" color="error" onClick={handleDecline}>
                Decline
              </Button>
            </Grid>
          </Grid>
        </Grid>
      </Grid>
    </div>
  );
}

export default LoanApproval;
