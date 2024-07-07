import { useParams, useRouter } from "next/navigation";

import {
  AppBar,
  Box,
  Button,
  IconButton,
  MenuItem,
  Select,
  Toolbar,
  Typography 
} from "@mui/material";

import MenuIcon from '@mui/icons-material/Menu';
import SettingsIcon from '@mui/icons-material/Settings';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';

interface TopBarProps {
  onMenuClick: () => void;
}

export default function TopBar({ onMenuClick }: TopBarProps): React.ReactElement<TopBarProps> {
  const router = useRouter();
  const params = useParams<{ spaceKey: string; }>();

  return (
    <AppBar position='sticky'>
      <Toolbar disableGutters variant='dense' sx={{ justifyContent: 'space-between' }}>
        <Box>
          <IconButton color='inherit' sx={{ marginLeft: '5px'}} onClick={onMenuClick}>
            <MenuIcon/>
          </IconButton>
          <Button color='inherit' onClick={() => router.push('/')}>
            <Typography>
              Task Manager
            </Typography>
          </Button>
          <Select
            value={decodeURIComponent(params.spaceKey)}
            sx={{ backgroundColor: 'white', height:'30px' }}
            onChange={(event) => {
              router.push(`/spaces/${event.target.value}/dashboard`);
            }}
          >
            {['Space 1', 'Space 2', 'Space 3'].map((spaceString) => (
              <MenuItem key={spaceString} value={spaceString}>
                {spaceString}
              </MenuItem>
            ))}
          </Select>
        </Box>
        <Box>
          <IconButton color='inherit'>
            <SettingsIcon/>
          </IconButton>
          <IconButton color='inherit'>
            <AccountCircleIcon/>
          </IconButton>
        </Box>
      </Toolbar>
    </AppBar>
  );
}
