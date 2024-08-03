import { Typography } from "@mui/material";

interface ITasksPageProps {
  params: {
    spaceKey: string;
  }
}

export default function TasksPage({ params }: ITasksPageProps) {
  return (
    <Typography className='fullscreen-center'>
        Tasks Page <br/>
        Selected Space: {decodeURIComponent(params.spaceKey)}
    </Typography>
  )
}