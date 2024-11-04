#!/bin/bash

export GET_TVSHOW_TOTAL_LENGTH_BIN="/c/Users/tomek/Desktop/GetTvShowTotalLength/GetTvShowTotalLength/bin/Debug/net8.0/GetTvShowTotalLength.exe"

show_name=()
show_length=()

while IFS= read -r showname; do
	{
		showlength=$("$GET_TVSHOW_TOTAL_LENGTH_BIN" "$showname")
		status_code=$?
		if [ $status_code -ne 0 ]; then
			echo "Could not get info for $showname."
		fi
		show_name+=("$showname")
		show_length+=("$showlength")
	}
done

wait

shortest_showlength=${show_length[0]}
shortest_showname=${show_name[0]}
longest_showlength=${show_length[0]}
longest_showname=${show_name[0]}

for i in "${!show_length[@]}"; do
	if [ "${show_length[i]}" -lt "$shortest_showlength" ]; then
		shortest_showlength=${show_length[i]}
		shortest_showname=${show_name[i]}
	fi
	if [ "${show_length[i]}" -gt "$longest_showlength" ]; then
		longest_showlength=${show_length[i]}
		longest_showname=${show_name[i]}
	fi
done

time_converter(){
	minutes=$1
	hours=$((minutes / 60))
	extra_minutes=$((minutes % 60))
	echo "${hours}h${extra_minutes}min"
}

shortest_showlength_converted=$(time_converter "shortest_showlength")
longest_showlength_converted=$(time_converter "longest_showlength")

echo "The shortest show: "
echo $shortest_showname
echo "($shortest_showlength_converted)"
echo "The longest show: "
echo $longest_showname
echo "($longest_showlength_converted)"

#echo "The shortest show: $shortest_showname ($shortest_showlength_converted)"
#echo "The longest show: $longest_showname ($longest_showlength_converted)"
