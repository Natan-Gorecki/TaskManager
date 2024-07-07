import { Typography } from "@mui/material";

export default function Home(): React.ReactElement {
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
      Welcome in Task Manager family! <br/>
      Please select your space to operate futher.
    </Typography>
  );
}
