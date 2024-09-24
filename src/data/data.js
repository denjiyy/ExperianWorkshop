/**
=========================================================
* Material Dashboard 2 PRO React - v2.1.0
=========================================================

* Product Page: https://www.creative-tim.com/product/material-dashboard-pro-react
* Copyright 2022 Creative Tim (https://www.creative-tim.com)

Coded by www.creative-tim.com

 =========================================================

* The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
*/

/* eslint-disable react/prop-types */
// ProductsList page components
import IdCell from "src/components/IdCell";
import DefaultCell from "src/components/DefaultCell";
import StatusCell from "src/components/StatusCell";
import CustomerCell from "src/components/CustomerCell";

// // Images
// import team1 from "assets/images/team-1.jpg";
// import team2 from "assets/images/team-2.jpg";
// import team3 from "assets/images/team-3.jpg";
// import team4 from "assets/images/team-4.jpg";
// import team5 from "assets/images/team-5.jpg";
// import ivana from "assets/images/ivana-squares.jpg";

const dataTableData = {
  columns: [
    { Header: "id", accessor: "id", Cell: ({ value }) => <IdCell id={value} /> },
    {
      Header: "date",
      accessor: "date",
      Cell: ({ value }) => <DefaultCell value={value} />,
    },
    {
      Header: "status",
      accessor: "status",
      Cell: ({ value }) => {
        let status;

        if (value === "paid") {
          status = <StatusCell icon="done" color="success" status="Paid" />;
        } else if (value === "refunded") {
          status = <StatusCell icon="replay" color="dark" status="Refunded" />;
        } else {
          status = <StatusCell icon="close" color="error" status="Canceled" />;
        }

        return status;
      },
    },
    {
      Header: "customer",
      accessor: "customer",
      Cell: ({ value: [name, data] }) => (
        <CustomerCell image={data.image} color={data.color || "dark"} name={name} />
      ),
    },
    {
      Header: "product",
      accessor: "product",
      Cell: ({ value }) => {
        const [name, data] = value;

        return (
          <DefaultCell
            value={typeof value === "string" ? value : name}
            suffix={data.suffix || false}
          />
        );
      },
    },
    { Header: "revenue", accessor: "revenue", Cell: ({ value }) => <DefaultCell value={value} /> },
  ],

  rows: [
    {
      id: "#10421",
      date: "1 Nov, 10:20 AM",
      status: "paid",
      customer: ["Orlando Imieto", ],
      product: "Nike Sport V2",
      revenue: "$140,20",
    },
    {
      id: "#10422",
      date: "1 Nov, 10:53 AM",
      status: "paid",
      customer: ["Alice Murinho", ],
      product: "Valvet T-shirt",
      revenue: "$42,00",
    },

  ],
};

export default dataTableData;
