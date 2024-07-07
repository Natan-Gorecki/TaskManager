import { Typography } from "@mui/material";

interface BoardPageProps {
  params: {
    spaceKey: string;
  }
}

export default function BoardPage({ params }: BoardPageProps): React.ReactElement {
  return (
    <Typography className='fullscreen-center'>
        Board Page <br/>
        Selected Space: {decodeURIComponent(params.spaceKey)}
    </Typography>
  )
}