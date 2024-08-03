import { Typography } from "@mui/material";

interface IBoardPageProps {
  params: {
    spaceKey: string;
  }
}

export default function BoardPage({ params }: IBoardPageProps): React.ReactElement {
  return (
    <Typography className='fullscreen-center'>
        Board Page <br/>
        Selected Space: {decodeURIComponent(params.spaceKey)}
    </Typography>
  )
}