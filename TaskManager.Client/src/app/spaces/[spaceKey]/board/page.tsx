import { Typography } from "@mui/material";

interface BoardPageProps {
  params: {
    spaceKey: string;
  }
}

export default function BoardPage({ params }: BoardPageProps): React.ReactElement {
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
        Board Page <br/>
        Selected Space: {decodeURIComponent(params.spaceKey)}
    </Typography>
  )
}