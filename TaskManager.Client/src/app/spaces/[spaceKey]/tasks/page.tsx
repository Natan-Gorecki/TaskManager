import { Typography } from "@mui/material";

interface TasksPageProps {
  params: {
    spaceKey: string;
  }
}

export default function TasksPage({ params }: TasksPageProps): React.ReactElement {
  return (
    <Typography className='fullscreen-center'>
        Tasks Page <br/>
        Selected Space: {decodeURIComponent(params.spaceKey)}
    </Typography>
  )
}