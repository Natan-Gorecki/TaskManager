import React from "react";
import { AppBar, Box, Button, IconButton, MenuItem, Select, SelectChangeEvent, Toolbar, Typography } from "@mui/material";
import { Theme } from '@mui/material/styles';

import MenuIcon from '@mui/icons-material/Menu';
import SettingsIcon from '@mui/icons-material/Settings';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';

import { useRouter } from "next/router";

interface TopBarProps {
  onMenuClick: () => void;
}

export default function TopBar({ onMenuClick }: TopBarProps): React.ReactElement<TopBarProps> {
  const router = useRouter();
  const [comboBoxValue, setComboBoxValue] = React.useState('Space 1');

  const handleComboBoxChange = (event: SelectChangeEvent<string>) => {
    setComboBoxValue(event.target.value as string);
  };

  return (
    <AppBar position='fixed' sx={{ zIndex: (theme: Theme) => theme.zIndex.drawer + 1 }}>
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
            value={comboBoxValue}
            sx={{ backgroundColor: 'white', height:'30px' }}
            onChange={handleComboBoxChange}
          >
            {['Space 1', 'Space 2', 'Space 3'].map((spaceString) => (
              <MenuItem value={spaceString}>
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
