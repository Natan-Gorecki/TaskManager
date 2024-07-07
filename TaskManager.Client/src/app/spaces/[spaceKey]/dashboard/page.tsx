import { Typography } from "@mui/material";

interface DashboardPageProps {
  params: {
    spaceKey: string;
  }
}

export default function DashboardPage({ params }: DashboardPageProps): React.ReactElement {
  return (
    <Typography sx={{
      position: 'absolute',
      top: 0,
      height: '100vh',
      width: '100vw',
      display: 'flex',
      flexDirection: 'column',
      justifyContent: 'center',
      alignItems: 'center',
      fontSize: '24px'
    }}>
        Dashboard Page <br/>
        Selected Space: {decodeURIComponent(params.spaceKey)}
    </Typography>
  )
}