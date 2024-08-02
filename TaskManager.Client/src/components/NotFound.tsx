import { Box, Typography } from "@mui/material";

interface INotFoundProps {
  errorMessage: string;
}

// nested not-found.tsx are not implemented yet
// https://github.com/vercel/next.js/discussions/48725

/*
<div style="font-family:system-ui,&quot;Segoe UI&quot;,Roboto,Helvetica,Arial,sans-serif,&quot;Apple Color Emoji&quot;,&quot;Segoe UI Emoji&quot;;height:100vh;text-align:center;display:flex;flex-direction:column;align-items:center;justify-content:center">
	<div>
		<style>body{color:#000;background:#fff;margin:0}.next-error-h1{border-right:1px solid rgba(0,0,0,.3)}@media (prefers-color-scheme:dark){body{color:#fff;background:#000}.next-error-h1{border-right:1px solid rgba(255,255,255,.3)}}</style>
		<h1 class="next-error-h1" style="display:inline-block;margin:0 20px 0 0;padding:0 23px 0 0;font-size:24px;font-weight:500;vertical-align:top;line-height:49px">404</h1>
		<div style="display:inline-block">
			<h2 style="font-size:14px;font-weight:400;line-height:49px;margin:0">This page could not be found.</h2>
		</div>
	</div>
</div>
.fullscreen-center {
  position: absolute;
  top: 0;
  height: 100vh;
  width: 100vw;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  font-size: 24px;
}
*/

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