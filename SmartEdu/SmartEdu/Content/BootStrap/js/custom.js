$(document).ready(function() {
    
    // retrive class
    $.ajax({
        type: "get",
        async: false,
        url: "php/student_classid_send.php",
        success: function(data) {
           $('#select_class, #edit_class').html(data);
        },
        error: function(){
            alert('The ajax call failed to produt details.');
        }
    });

    //retrive teacher
    $.ajax({
        type: "get",
        async: false,
        url: "php/student_teacherid_send.php",
        success: function(data) {
           $('#select_teacher').html(data);
        },
        error: function(){
            alert('The ajax call failed to produt details.');
        }
    });


    //retrive student list
  $("#submit").click(function(){
        var class_id = $("#select_class").val();
        if (class_id==0) {
                // alert("select class");
        } else {

                $.ajax({
                      type: 'POST',
                      url: "php/student_list_send.php",
                      data: "class_id="+class_id,
                      dataType: "html",
                      success: function(data) {
                        $('#display-table').show();
                        $('#row').html(data);

                        $('a').click(function() {

                            var action_id = $(this).attr('id');
                            var action_class = $(this).attr('class');
                            
                            // alert(action_id);

                            if(action_class=='view-profile') {
                                $.ajax({
                                  type: 'POST',
                                  url: "php/view_profile.php",
                                  data: "class_id="+class_id+"&action_id="+action_id,
                                  dataType: "html",
                                  success: function(data) {
                                    $('#view-fulprofile').html(data);

                                  } 
                                });
                                  
                            } else if(action_class=='academic-result') {
                                $.ajax({
                                  type: 'POST',
                                  url: "php/view_academic_profile.php",
                                  data: "class_id="+class_id+"&action_id="+action_id,
                                  dataType: "html",
                                  success: function(data) {
                                    $('#view_academic_result').html(data);
                                    

                                  } 
                                });
                                  
                            }  else if (action_class=='edit') {
                                $.ajax({
                                  type: 'POST',
                                  dataType: "json",
                                  url: "php/retrive_edit_userinfo.php",
                                  data: "class_id="+class_id+"&action_id="+action_id,
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
                                    $("input[ name = father_name ]").val( response.father_name);
                                    $("input[ name = mother_name ]").val( response.mother_name);
                                    $("input[ name = roll ]").val( response.roll);

                                    $("#present_class").html("<option value='"+response.class_id+"'>"+'Class '+response.class_name+"</option>");
                                    $('#student_id').val(response.student_id);

                                    
                                    $("button[name = save_change ]").click(function() {

                                      var edit_formData = new FormData($('#edit_signupForm')[0]);
                                      

                                      $.ajax({
                                        type: 'POST',
                                        url: "php/update_edit_student.php",
                                        data: edit_formData,
                                        dataType: "html",
                                        success: function(data) {
                                          alert(data);
                                          window.location.href='student_list.php';
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
                                    url: "php/delete_student.php",
                                    data: "class_id="+class_id+"&action_id="+action_id,
                                    dataType: "text",
                                    success: function(data) {
                                      alert(data);
                                      window.location.href='student_list.php';
                                      
                                      return false;

                                    } 
                                });


                            } //end delete else

                        }); 

                        }
                });
                // $.post("php/student_list_send.php", {class_id:class_id}, function(data) {
                //      $('#responseContainer').html(data);
                // });

        }


  });
    


});