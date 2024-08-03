import { Typography } from "@mui/material";

interface IDashboardPageProps {
  params: {
    spaceKey: string;
  }
}

export default function DashboardPage({ params }: IDashboardPageProps) {
  return (
    <Typography className='fullscreen-center'>
        Dashboard Page <br/>
        Selected Space: {decodeURIComponent(params.spaceKey)}
    </Typography>
  )
}