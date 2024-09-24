import { Carousel } from "react-responsive-carousel";
import "react-responsive-carousel/lib/styles/carousel.min.css";
import { Paper, Typography, Box, Button, IconButton } from "@mui/material";
import ArrowBackIosIcon from "@mui/icons-material/ArrowBackIos";
import ArrowForwardIosIcon from "@mui/icons-material/ArrowForwardIos";
import Stack from "@mui/material/Stack";
import { styled } from "@mui/material/styles";
import LinearProgress, {
  linearProgressClasses,
} from "@mui/material/LinearProgress";
import "./style.css";
import React, { useState, useEffect } from "react";

const accountData = [
  {
    amount: 5000,
    accountNum: "6769649ILG",
  },
  {
    amount: 0,
    accountNum: "832873287h",
  },
];
const loanData = [
  {
    name: "John Doe",
    age: 26,
    weeks: 78,
    amount: 5000,
    score1: 85,
    score2: 92,
    type: "Personal Loan",
  },
  {
    name: "Goso",
    age: 6,
    weeks: 78,
    amount: 800,
    score1: 55,
    score2: 0,
    type: "Mortgage Loan",
  },
  {
    name: "Goso",
    age: 6,
    weeks: 78,
    amount: 800,
    score1: 55,
    score2: 0,
    type: "Mortgage Loan",
  },
  {
    name: "Goso",
    age: 6,
    weeks: 78,
    amount: 800,
    score1: 55,
    score2: 0,
    type: "Mortgage Loan",
  },
  {
    name: "Goso",
    age: 6,
    weeks: 78,
    amount: 800,
    score1: 55,
    score2: 0,
    type: "Mortgage Loan",
  },
  {
    name: "Goso",
    age: 6,
    weeks: 78,
    amount: 800,
    score1: 55,
    score2: 0,
    type: "Mortgage Loan",
  },
];

const numAccounts = accountData.length;
const showMultipleAccounts = numAccounts >= 3; // Display 3 slides only if there are 3 or more items
const numLoans = loanData.length;
const showMultipleLoans = numLoans >= 3; // Display 3 slides only if there are 3 or more items

const BorderLinearProgress = styled(LinearProgress)(({ theme }) => ({
  height: 10,
  borderRadius: 5,
  [`&.${linearProgressClasses.colorPrimary}`]: {
    backgroundColor: theme.palette.grey[200],
    ...theme.applyStyles("dark", {
      backgroundColor: theme.palette.grey[800],
    }),
  },
  [`& .${linearProgressClasses.bar}`]: {
    borderRadius: 5,
    backgroundColor: "#1a90ff",
    ...theme.applyStyles("dark", {
      backgroundColor: "#308fe8",
    }),
  },
}));

function ResponsiveCarousel({ accountData, loanData }) {}

function HomePage() {
  const [showMultipleAccounts, setShowMultipleAccounts] = useState(true);
  const [showMultipleLoans, setShowMultipleLoans] = useState(true);

  // Dynamically adjust the carousel behavior based on screen width
  useEffect(() => {
    const handleResize = () => {
      if (window.innerWidth <= 600) {
        setShowMultipleAccounts(false);
        setShowMultipleLoans(false);
      } else {
        setShowMultipleAccounts(true);
        setShowMultipleLoans(true);
      }
    };

    window.addEventListener("resize", handleResize);
    handleResize(); // Run on initial load to set correct state

    return () => window.removeEventListener("resize", handleResize);
  }, []);
  return (
    <div className="carousel-box">
      <Typography variant="h5">Account Details</Typography>
      <div className="carousel">
        <Carousel
          showStatus={false}
          showThumbs={false}
          centerMode={showMultipleAccounts}
          centerSlidePercentage={showMultipleAccounts ? 33.33 : 100}
          infiniteLoop={showMultipleAccounts}
          autoPlay={showMultipleAccounts}
          interval={3000}
          renderArrowPrev={(onClickHandler, hasPrev) =>
            hasPrev && (
              <IconButton
                onClick={onClickHandler}
                sx={{
                  position: "absolute",
                  left: 15,
                  top: "50%",
                  zIndex: 10,
                  backgroundColor: "#fff",
                }}
              >
                <ArrowBackIosIcon />
              </IconButton>
            )
          }
          renderArrowNext={(onClickHandler, hasNext) =>
            hasNext && (
              <IconButton
                onClick={onClickHandler}
                sx={{
                  position: "absolute",
                  right: 15,
                  top: "50%",
                  zIndex: 10,
                  backgroundColor: "#fff",
                }}
              >
                <ArrowForwardIosIcon />
              </IconButton>
            )
          }
        >
          {accountData.map((account, index) => (
            <Box
              key={index}
              p={2}
              sx={{
                minWidth: "250px",
                maxWidth: "300px",
                margin: "0 auto",
                "@media (max-width: 600px)": {
                  minWidth: "100%",
                  padding: "8px",
                },
              }}
            >
              <Paper elevation={3} sx={{ p: 4 }}>
                <Typography variant="body1">
                  <strong>Amount:</strong> ${account.amount}
                </Typography>
                <Typography variant="body1">
                  <strong>Account Number:</strong> {account.accountNum}
                </Typography>
                <Box mt={2}>
                  <Button variant="contained" color="primary" sx={{ mr: 1 }}>
                    See transactions
                  </Button>
                </Box>
              </Paper>
            </Box>
          ))}
        </Carousel>
      </div>

      <Typography variant="h5">Loan Details</Typography>
      <div className="carousel">
        <Carousel
          showStatus={false}
          showThumbs={false}
          centerMode={showMultipleLoans}
          centerSlidePercentage={showMultipleLoans ? 33.33 : 100}
          infiniteLoop={showMultipleLoans}
          autoPlay={showMultipleLoans}
          interval={3000}
          renderArrowPrev={(onClickHandler, hasPrev) =>
            hasPrev && (
              <IconButton
                onClick={onClickHandler}
                sx={{
                  position: "absolute",
                  left: 15,
                  top: "50%",
                  zIndex: 10,
                  backgroundColor: "#fff",
                }}
              >
                <ArrowBackIosIcon />
              </IconButton>
            )
          }
          renderArrowNext={(onClickHandler, hasNext) =>
            hasNext && (
              <IconButton
                onClick={onClickHandler}
                sx={{
                  position: "absolute",
                  right: 15,
                  top: "50%",
                  zIndex: 10,
                  backgroundColor: "#fff",
                }}
              >
                <ArrowForwardIosIcon />
              </IconButton>
            )
          }
        >
          {loanData.map((loan, index) => (
            <Box
              key={index}
              p={2}
              sx={{
                minWidth: "250px",
                maxWidth: "300px",
                margin: "0 auto",
                "@media (max-width: 600px)": {
                  minWidth: "100%",
                  padding: "8px",
                },
              }}
            >
              <Paper elevation={3} sx={{ p: 4 }}>
                <Typography variant="body1">
                  <strong>Amount:</strong> ${loan.amount}
                </Typography>
                <Typography variant="body1">
                  <strong>Type:</strong> {loan.type}
                </Typography>
                <Stack spacing={2} sx={{ flexGrow: 1 }}>
                  <br />
                  <BorderLinearProgress variant="determinate" value={30} />
                </Stack>
                <Box mt={2}>
                  <Button variant="contained" color="primary" sx={{ mr: 1 }}>
                    More info
                  </Button>
                </Box>
              </Paper>
            </Box>
          ))}
        </Carousel>
      </div>
    </div>
  );
}

export default HomePage;
