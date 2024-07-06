"use client"

import { Typography } from "@mui/material";

export default function SummaryPage(): React.ReactElement {
  return (
    <Typography sx={{
      position: 'absolute',
      top: 0,
      height: '100vh',
      width: '100vw',
      display: 'flex',
      justifyContent: 'center',
      alignItems: 'center',
      fontSize: '24px'
    }}>
      Summary Page
    </Typography>
  )
}