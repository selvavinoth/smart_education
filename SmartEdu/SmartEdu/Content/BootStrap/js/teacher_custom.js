$(document).ready(function() {

	$.ajax({
          type: 'GET',
          url: "php/teacher_list_send.php",
          dataType: "html",
          success: function(data) {
            // $('#display-table').show();
            $('#row').html(data);

            $('a').click(function() {

                var action_id = $(this).attr('id');
                var action_class = $(this).attr('class');
                
                // alert(action_id);

                if(action_class=='view-profile') {
                    $.ajax({
                      type: 'POST',
                      url: "php/view_teacher_profile.php",
                      data: "action_id="+action_id,
                      dataType: "html",
                      success: function(data) {
                        $('#view-fullprofile').html(data);

                      } 
                    });
                      
                } else if (action_class=='edit') {
                    $.ajax({
                      type: 'POST',
                      dataType: "json",
                      url: "php/retrive_edit_teacherinfo.php",
                      data: "action_id="+action_id,
                      dataType: "html",
                      success: function(data) {
                        var response = jQuery.parseJSON(data);
                        $("input[ name = name ]").val( response.name);
                        $("input[ name = birthday ]").val( response.birthday);
                        $("input[ name = sex ]").val( response.sex);
                        $("input[ name = bloodgroup ]").val( response.blood_group);
                        $("input[ name = religion ]").val( response.religion);
                        $("input[ name = address ]").val( response.address);
                        $("input[ name = phone ]").val( response.phone);
                        $("input[ name = email ]").val( response.email);  
                        // $("input[ type = file ]").change(response.image);
                        $('#teacher_id').val(response.teacher_id);
                        
                        $("button[name = save_change ]").click(function() {
                          // var teacher_id = response.teacher_id;
                          var edit_formData = new FormData($('#edit_signupForm')[0]);

                          $.ajax({
                            type: 'POST',
                            url: "php/update_edit_teacher.php",
                            data: edit_formData,
                            dataType: "html",
                            success: function(data) {
                              alert(data);
                              window.location.href='teacher_list.php';
                              return false;


                            },
                              cache: false,
                              contentType: false,
                              processData: false 
                          });


                        });

                      } 
                    });
                }
                // end else if edit..
                else if (action_class=='delete') {
                    $.ajax({
                        type: 'POST',
                        url: "php/delete_teacher.php",
                        data: "action_id="+action_id,
                        dataType: "text",
                        success: function(data) {
                          alert(data);
                          window.location.href='teacher_list.php';
                          
                          return false;

                        } 
                    });


                } //end delete else

            }); 

          }
    });

















});