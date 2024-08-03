import { Box, Typography } from "@mui/material";

interface INotFoundProps {
  errorMessage: string;
}

// nested not-found.tsx are not implemented yet
// https://github.com/vercel/next.js/discussions/48725
export default function NotFound({ errorMessage }: INotFoundProps) {
  return (
    <Box className='fullscreen-center'>
      <Typography variant='h1' className='notfound-404'>
        404
      </Typography>
      <Box sx={{display: 'inline-block'}}>
        <Typography variant='h2' className='notfound-errormessage'>
          {errorMessage}
        </Typography>
      </Box>
    </Box>
  )
}