import { Typography } from "@mui/material";

interface DashboardPageProps {
  params: {
    spaceKey: string;
  }
}

export default function DashboardPage({ params }: DashboardPageProps): React.ReactElement {
  return (
    <Typography className='fullscreen-center'>
        Dashboard Page <br/>
        Selected Space: {decodeURIComponent(params.spaceKey)}
    </Typography>
  )
}